using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI.ViewModels
{
    public class BookItem //这个数据结构用于组成一个ObservableCollection<BookItem>()
    {//用于在ui界面中显示一个列表型的图书信息
        public String ID { get; set; }  //按递增顺序排列
        public String BookName { get; set; }
        public String BookAccessCode { get; set; }
        public String BookRFIDCode { get; set; }
    }
    public class RecodeBookLocationViewModel : INavigationAware
    {
        IUnityContainer container;
        IRegionManager regionManager;
        IEventAggregator eventAggregator;
        IDispatcherService dispatcherService;
        ObservableCollection<BookItem> bookItemList;
        public RecodeBookLocationViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            this.dispatcherService = container.Resolve<IDispatcherService>();
            //### ui中用到的变量
            this.bookItemList = new ObservableCollection<BookItem>();
            //this.bookItemList.Add(new BookItem() { ID = "1", BookName="123",BookAccessCode="TP123",BookRFIDCode="0x123"});
 
        }
        public ObservableCollection<BookItem> BookItemList
        {
            get { return this.bookItemList; }
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
                        this.bookItemList.Add(new BookItem() { ID = "1", BookName = bookNameList[i], BookAccessCode = bookAccessCodeList[i], BookRFIDCode = bookRfidList[i] });
                    }
                });
            }
            if (newItem.shelfRfidList.Count() != 0)
            {
                IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();
                String shelfName = bookLocationService.getShelfNameByShelfRfid(newItem.shelfRfidList[0]);
            }           

            //throw new NotImplementedException();
            //开始处理读取到的数据，并显示在图形界面中
            //begin at this next time
            
            //this.bookItemList.Add(new BookItem() { ID = "2", BookName = "123", BookAccessCode = "TP123", BookRFIDCode = "0x123" });
            //this.bookItemList.Add(new BookItem() { ID = "3", BookName = "123", BookAccessCode = "TP123", BookRFIDCode = "0x123" });
            //this.bookItemList.Add(new BookItem() { ID = "4", BookName = "123", BookAccessCode = "TP123", BookRFIDCode = "0x123" });

        }
    }
}
