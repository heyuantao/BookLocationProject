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
    public class BookItemOfWrongLocation:ICloneable //这个数据结构用于组成一个ObservableCollection<BookItem>()
    {//用于在ui界面中显示一个列表型的图书信息
        public String ID { get; set; }  //按递增顺序排列
        public String BookName { get; set; }
        public String BookAccessCode { get; set; }
        public String BookRFIDCode { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class WrongBookLocationViewModel : INavigationAware, INotifyPropertyChanged  
    {
        IUnityContainer container;
        IRegionManager regionManager;
        IEventAggregator eventAggregator;
        IDispatcherService dispatcherService;
        //该控件使用到的变量
        String shelfRfid = "";//扫描到的书架的rfid标签，用于该模块内部逻辑使用
        String shelfLocationString = "";//用于在UI中显示的"当前书架位置"
        ObservableCollection<BookItemOfWrongLocation> onThisShelfAllBookList;
        ObservableCollection<BookItemOfWrongLocation> notOnThisShelfBookList;
        ICommand wrongBookLocationCleanAllCommand; //对应按钮的事件

        public WrongBookLocationViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            this.dispatcherService = container.Resolve<IDispatcherService>();
            //初始化控件用到的变量
            this.onThisShelfAllBookList = new ObservableCollection<BookItemOfWrongLocation>();
            this.notOnThisShelfBookList = new ObservableCollection<BookItemOfWrongLocation>();
            this.wrongBookLocationCleanAllCommand = new DelegateCommand(wrongBookLocationCleanAllCommandExecute, wrongBookLocationCleanAllCommandCanExecute);
        }
        //program logic
        private bool wrongBookLocationCleanAllCommandCanExecute()
        {
            //判断清除按钮是否可以被激活
            if (! String.IsNullOrEmpty(this.ShelfLocationString))
            {
                return true;
            }
            if (this.OnThisShelfAllBookList.Count() > 0)
            {
                return true;
            }
            if (this.NotOnThisShelfBookList.Count() > 0)
            {
                return true;
            }
            return false;
        }
        private void wrongBookLocationCleanAllCommandExecute()
        {//清除UI上的内容，并刷新UI
            this.ShelfLocationString = "";
            this.OnThisShelfAllBookList.Clear();
            this.OnThisShelfAllBookList = this.OnThisShelfAllBookList;
            this.NotOnThisShelfBookList.Clear();
            this.NotOnThisShelfBookList = this.NotOnThisShelfBookList;
        }
        private void handleNewItemFromRFID(RFIDContent newItem)
        {
            if (newItem.bookRfidList.Count() != 0)
            {//只有当当前在某个书架上的图书不为空的时候才继续处理
                IBookInformationService bookInformationService = container.Resolve<IBookInformationService>();
                IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();
                if(!String.IsNullOrEmpty(this.ShelfLocationString))//  书架位置信息不空，这时有必要继续执行
                {
                    //如果出现不在该架位的图书，则提示该书不属于当前书架
                    foreach (String bookrfid in newItem.bookRfidList)
                    {
                        String shelfRfid=bookLocationService.getShelfRfidbyBookRfid(bookrfid);
                        if (shelfRfid != this.shelfRfid)
                        {
                            try
                            {
                                List<String> bookNameList = bookInformationService.getBookNameListByRfidList(new List<String>() { bookrfid });
                                String bookName = bookNameList[0];
                                String message = "图书名称:" + bookName + "图书扫描码:" + bookrfid + "不属于该书架";
                                MessageBox.Show(message);
                            }
                            catch (Exception)
                            {
                                String message = "查询数据库出错，图书扫描码为:" + bookrfid+",请检查是否登记该图书 ！";
                                MessageBox.Show(message);
                            }                            
                        }
                    }
                    //图书的rfid是否有重复，删除掉重复的部分，并刷新UI，但ID编号可能不连续，重新更新ID编号                 
                    this.dispatcherService.Dispatch(() =>
                    {
                        List<BookItemOfWrongLocation> tempList = new List<BookItemOfWrongLocation>();
                        //把原始的datagrid中的内容存入临时的列表中，并清空该列表
                        foreach (BookItemOfWrongLocation item in this.NotOnThisShelfBookList)
                        {
                            tempList.Add(item);
                        }
                        this.NotOnThisShelfBookList.Clear();
                        //查看临时列表中与新读入标签重复的地方
                        int index=0;
                        while(true){
                            if (index >= tempList.Count())
                            {
                                break;
                            }
                            BookItemOfWrongLocation item = tempList[index];
                            if (newItem.bookRfidList.Contains(item.BookRFIDCode))
                            {
                                tempList.Remove(item);
                            }
                            index = index + 1;
                        }
                        //把临时标签的内容复制入datagrid,并重新计算ID,因为ID可能经过删除有不连续的现象
                        int count = 1;
                        foreach (BookItemOfWrongLocation item in tempList)
                        {
                            item.ID = Convert.ToString(count);
                            this.NotOnThisShelfBookList.Add(item);
                            count = count + 1;
                        }
                        //刷新UI
                        this.NotOnThisShelfBookList = this.NotOnThisShelfBookList;

                    });
                }
            }
            if (newItem.shelfRfidList.Count() != 0) //发现有新的书架信息
            {
                //清除原有信息
                this.dispatcherService.Dispatch(() =>
                {
                    this.OnThisShelfAllBookList.Clear();
                    this.NotOnThisShelfBookList.Clear();
                });          
                //在书架位置的文本框内显示数据的位置
                IBookInformationService bookInformationService = container.Resolve<IBookInformationService>();
                IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();
                this.shelfRfid = newItem.shelfRfidList[0];
                this.ShelfLocationString = bookLocationService.getShelfNameByShelfRfid(newItem.shelfRfidList[0]);                
                //查询数据库，在本书架应有图书中显示所有的图书
                List<String> bookRfidList = bookLocationService.getBookRfidListOnShelfRfid(this.shelfRfid);
                List<String> bookNameList=bookInformationService.getBookNameListByRfidList(bookRfidList);
                List<String> bookAccessCodeList=bookInformationService.getBookAccessCodeListByRfidList(bookRfidList);
                if ((bookRfidList.Count() == bookNameList.Count()) && (bookNameList.Count() == bookAccessCodeList.Count()))
                {//查询到的图书名称和图书索取码必须与图书rfid字符串长度相等
                    for (int i = 0; i < bookRfidList.Count(); i++)
                    {

                        BookItemOfWrongLocation newBookItem = new BookItemOfWrongLocation() 
                        {
                            ID=Convert.ToString(i+1),
                            BookName=bookNameList[i],
                            BookAccessCode=bookAccessCodeList[i],
                            BookRFIDCode=bookRfidList[i]
                        };
                        BookItemOfWrongLocation newBookItemInOtherGrid = (BookItemOfWrongLocation)newBookItem.Clone();
                        this.dispatcherService.Dispatch(() =>
                        {
                            this.OnThisShelfAllBookList.Add(newBookItem);
                            this.notOnThisShelfBookList.Add(newBookItemInOtherGrid);
                        });
                    }
                    this.dispatcherService.Dispatch(() =>
                    {
                        this.OnThisShelfAllBookList = this.OnThisShelfAllBookList;
                        this.NotOnThisShelfBookList = this.NotOnThisShelfBookList;
                    });
                }

            }
        }
        private void handleErrorFromRFID(string errorMessage)
        {
            MessageBox.Show(errorMessage);
            //当串口设置出错时提示信息并转移到串口设置界面
            //this.regionManager.RequestNavigate("MainRegion", new Uri("SystemSettingView", UriKind.Relative));
            //NavBarViewModel vm = this.container.Resolve<NavBarViewModel>();
            //vm.switchSystemSettingView();
        }
        //Bind variable and ICommand on UI
        public String ShelfLocationString
        {
            get { return this.shelfLocationString; }
            set {
                this.shelfLocationString = value; this.OnPropertyChanged("ShelfLocationString");
                this.dispatcherService.Dispatch(() =>
                {
                    ((DelegateCommand)this.WrongBookLocationCleanAllCommand).RaiseCanExecuteChanged();
                });
            }
        }
        public ICommand WrongBookLocationCleanAllCommand
        {
            get { return this.wrongBookLocationCleanAllCommand; }
        }
        public ObservableCollection<BookItemOfWrongLocation> OnThisShelfAllBookList
        {
            get { return this.onThisShelfAllBookList; }
            set { 
                this.onThisShelfAllBookList = value; 
                this.OnPropertyChanged("OnThisShelfAllBookList");
                this.dispatcherService.Dispatch(() =>
                {
                    ((DelegateCommand)this.WrongBookLocationCleanAllCommand).RaiseCanExecuteChanged();
                });
            }
        }
        public ObservableCollection<BookItemOfWrongLocation> NotOnThisShelfBookList
        {
            get { return this.notOnThisShelfBookList; }
            set { 
                this.notOnThisShelfBookList = value;
                this.OnPropertyChanged("NotOnThisShelfBookList");
                this.dispatcherService.Dispatch(() =>
                {
                    ((DelegateCommand)this.WrongBookLocationCleanAllCommand).RaiseCanExecuteChanged();
                });
            }
        }

        //########################################
        //########################################
        // system method,not change it
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
            //ISerialService serialService = container.Resolve<ISerialService>();
            IBookInformationService bookInformationService = container.Resolve<IBookInformationService>();
            IBookLocationService bookLocationService = container.Resolve<IBookLocationService>();
            IRFIDService rfidService = container.Resolve<IRFIDService>();

            //开始读取RFID的信息，并查询数据库
            eventAggregator.GetEvent<RFIDNewItemEvent>().Subscribe(handleNewItemFromRFID);
            //开始订阅RFID服务发出的事件，这个事件是扫描到的条码的信息，在该view非激活时务必取消此事件的订阅
            eventAggregator.GetEvent<RFIDHardwareEvent>().Subscribe(handleErrorFromRFID);

            rfidService.start();//开启rfid的后台扫描线程

        }


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
