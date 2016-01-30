using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UI.ViewModels
{
    public class BookItem //这个数据结构用于组成一个ObservableCollection<BookItem>()
    {//用于在ui界面中显示一个列表型的图书信息
        public String ID { get; set; }  //按递增顺序排列
        public String BookName { get; set; }
        public String BookAccessCode { get; set; }
        public String BookRFIDCode { get; set; }
    }
    public class RecodeBookLocationViewModel : INavigationAware,INotifyPropertyChanged  
    {
        IUnityContainer container;
        IRegionManager regionManager;
        IEventAggregator eventAggregator;
        IDispatcherService dispatcherService;
        ObservableCollection<BookItem> bookItemList; //图形界面中显示的图书列表
        int bookItemCount; //图形界面中图书列表中的ID值，每次增加1，初始值为1
        String shelfName;//图形界面中的书架信息
        String shelfRfid;
        public RecodeBookLocationViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            this.dispatcherService = container.Resolve<IDispatcherService>();
            //### ui中用到的变量
            this.bookItemList = new ObservableCollection<BookItem>();
            this.bookItemCount = 1;
            this.shelfName = "";
            this.shelfRfid = "";
            //this.bookItemList.Add(new BookItem() { ID = "1", BookName="123",BookAccessCode="TP123",BookRFIDCode="0x123"});
 
        }
        public ObservableCollection<BookItem> BookItemList
        {
            get { return this.bookItemList; }
            set { this.bookItemList = value; this.OnPropertyChanged("BookItemList"); }
        }
        public String ShelfName
        {
            get { return this.shelfName; }
            set { this.shelfName = value; this.OnPropertyChanged("ShelfName"); }
        }
        public ICommand RecodeBookLocationCleanBookListCommand
        {
            get { return new DelegateCommand(onRecodeBookLocationCleanBookListCommandExecute); }
        }
        public ICommand RecodeBookLocationAddBookListCommand
        {
            get { return new DelegateCommand(onRecodeBookLocationAddBookListCommandExecute); }
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            //取消订阅该事件,在此之前先关闭rfid后台扫描线程
            IRFIDService rfidService = container.Resolve<IRFIDService>();
            rfidService.stop();
            eventAggregator.GetEvent<RFIDNewItemEvent>().Unsubscribe(handleNewItemFromRFID);
            eventAggregator.GetEvent<RFIDHardwareEvent>().Unsubscribe(handleErrorFromRFID);

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            ISerialService serialService = container.Resolve<ISerialService>();
            IBookInformationService bookInformationService = container.Resolve<IBookInformationService>();
            IBookLocationService bookLocationService = container.Resolve<IBookLocationService>();
            IRFIDService rfidService = container.Resolve<IRFIDService>();
            
            //开始读取RFID的信息，并查询数据库
            eventAggregator.GetEvent<RFIDNewItemEvent>().Subscribe(handleNewItemFromRFID);
            //开始订阅RFID服务发出的事件，这个事件是扫描到的条码的信息，在该view非激活时务必取消此事件的订阅
            eventAggregator.GetEvent<RFIDHardwareEvent>().Subscribe(handleErrorFromRFID);
            
            rfidService.start();//开启rfid的后台扫描线程
            
        }

        private void handleErrorFromRFID(string errorMessage)
        {
            MessageBox.Show(errorMessage);
            //当串口设置出错时提示信息并转移到串口设置界面
            this.regionManager.RequestNavigate("MainRegion", new Uri("SystemSettingView", UriKind.Relative));

        }

        private void handleNewItemFromRFID(RFIDContent newItem)
        {
            //判断是不是没有数据
            if (newItem.bookRfidList.Count() != 0)
            {
                //从newItem中获取图书数据并开始查询
                IBookInformationService bookInformationService = this.container.Resolve<IBookInformationService>();
                List<String> bookRfidList = newItem.bookRfidList;
                //查看图形界面中的图书列表，即bookItemList与bookRfidList是否有重复，消除掉重复的部分
                foreach (BookItem item in this.bookItemList)
                {
                    if(bookRfidList.Contains(item.BookRFIDCode))
                    {
                        bookRfidList.Remove(item.BookRFIDCode);//删除后链表可能为空
                    }
                }
                if (bookRfidList.Count() == 0) { return; }

                List<String> bookNameList = bookInformationService.getBookNameListByRfidList(bookRfidList);
                List<String> bookAccessCodeList = bookInformationService.getBookAccessCodeListByRfidList(bookRfidList);
                //这里有个bug，就是三个列表长度不同，研究下为什么，暂时先用比较长度的方式来解决
                if (bookRfidList.Count() != bookNameList.Count()){return;}
                if (bookRfidList.Count() != bookAccessCodeList.Count()) { return; }
                if (bookNameList.Count() != bookAccessCodeList.Count()) { return; }
                this.dispatcherService.Dispatch(() =>
                {
                    for (int i = 0; i < bookRfidList.Count(); i++)
                    {
                        this.bookItemList.Add(new BookItem() { 
                            ID = Convert.ToString(this.bookItemCount), 
                            BookName = bookNameList[i], 
                            BookAccessCode = bookAccessCodeList[i], 
                            BookRFIDCode = bookRfidList[i] });
                        this.bookItemCount = this.bookItemCount + 1;
                    }
                });
            }
            if (newItem.shelfRfidList.Count() != 0)
            {
                IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();
                this.ShelfName = bookLocationService.getShelfNameByShelfRfid(newItem.shelfRfidList[0]);
                this.shelfRfid = newItem.shelfRfidList[0];
            }           

            //throw new NotImplementedException();
            //开始处理读取到的数据，并显示在图形界面中
            //begin at this next time
            
            //this.bookItemList.Add(new BookItem() { ID = "2", BookName = "123", BookAccessCode = "TP123", BookRFIDCode = "0x123" });
            //this.bookItemList.Add(new BookItem() { ID = "3", BookName = "123", BookAccessCode = "TP123", BookRFIDCode = "0x123" });
            //this.bookItemList.Add(new BookItem() { ID = "4", BookName = "123", BookAccessCode = "TP123", BookRFIDCode = "0x123" });

        }

        private void onRecodeBookLocationCleanBookListCommandExecute()
        {
            //throw new NotImplementedException();
            this.clearBookListAndShelfInformation();
        }
        private void onRecodeBookLocationAddBookListCommandExecute()
        {
            IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();
            String currentShelfRfid = this.shelfRfid;
            List<String> bookRfidList=new List<String>();

            //把新的信息加入数据库
            foreach (BookItem oneBook in this.BookItemList)
            {
                bookRfidList.Add(oneBook.BookRFIDCode);
            }
            bookLocationService.setBookRfidListOnShelfRfid(currentShelfRfid, bookRfidList);
            this.clearBookListAndShelfInformation();
            //throw new NotImplementedException();
            MessageBox.Show("添加图书信息成功！");
        }
        private void clearBookListAndShelfInformation()
        {
            this.BookItemList = new ObservableCollection<BookItem>();
            this.bookItemCount = 1;
            
            this.ShelfName = "";
            this.shelfRfid = "";
        }
        //########################################
        //########################################
        // system method,not change it
        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
