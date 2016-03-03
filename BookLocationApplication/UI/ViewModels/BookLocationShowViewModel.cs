using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Services;

namespace UI.ViewModels
{
    public class BookLocationShowViewModel : INavigationAware, INotifyPropertyChanged  
    {
        IUnityContainer container;
        IRegionManager regionManager;
        IEventAggregator eventAggregator;
        IDispatcherService dispatcherService;
        //在UI中显示的变量
        String bookName;
        String bookAccessCode;
        String bookLocation;
        //按钮的处理事件DelegateCommand
        ICommand bookLocationShowClearCommand;
        //两个Canvas，用于显示地图信息
        DrawMapService libraryMapService;
        public BookLocationShowViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            this.dispatcherService = container.Resolve<IDispatcherService>();
            //初始化UI的变量
            this.bookName = ""; this.bookAccessCode = ""; this.bookLocation = "";
            //初始化两个地图画板
            this.libraryMapService = this.container.Resolve<DrawMapService>();
            this.libraryMapService.initOneShapMap(150, 400, 150, 400);
            this.libraryMapService.drawOneShapeMapBackground();
            //this.LibraryMapService = this.LibraryMapService; //通知更新UI

            //this.libraryMapService.initLibraryShelfMap(600,400,25000,22000);
            this.libraryMapService.initLibraryShelfMap(660, 400, 25000, 22000);
            this.LibraryMapService = this.LibraryMapService;//通知更新UI

        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            IRFIDService rfidService = container.Resolve<IRFIDService>();
            rfidService.stop();
            eventAggregator.GetEvent<RFIDNewItemEvent>().Unsubscribe(handleNewItemFromRFID);
            eventAggregator.GetEvent<RFIDHardwareEvent>().Unsubscribe(handleErrorFromRFID);
            eventAggregator.GetEvent<DatabaseEvent>().Unsubscribe(handleErrorFromDatabase);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //ISerialService serialService = container.Resolve<ISerialService>();
            //IBookInformationService bookInformationService = container.Resolve<IBookInformationService>();
            //IBookLocationService bookLocationService = container.Resolve<IBookLocationService>();
            IRFIDService rfidService = container.Resolve<IRFIDService>();

            //开始读取RFID的信息，并查询数据库
            eventAggregator.GetEvent<RFIDNewItemEvent>().Subscribe(handleNewItemFromRFID);
            //开始订阅RFID服务发出的事件，这个事件是扫描到的条码的信息，在该view非激活时务必取消此事件的订阅
            eventAggregator.GetEvent<RFIDHardwareEvent>().Subscribe(handleErrorFromRFID);
            eventAggregator.GetEvent<DatabaseEvent>().Subscribe(handleErrorFromDatabase);

            rfidService.start();//开启rfid的后台扫描线程
        }

        
        public String BookName
        {
            get { return this.bookName; }
            set { 
                this.bookName = value; 
                this.OnPropertyChanged("BookName");
                this.dispatcherService.Dispatch(() =>
                {
                    ((DelegateCommand)this.BookLocationShowClearCommand).RaiseCanExecuteChanged();
                });
            }
        }
        public String BookAccessCode
        {
            get { return this.bookAccessCode; }
            set { 
                this.bookAccessCode = value; 
                this.OnPropertyChanged("BookAccessCode");
                ((DelegateCommand)this.BookLocationShowClearCommand).RaiseCanExecuteChanged();

            }
        }
        public String BookLocation
        {
            get { return this.bookLocation; }
            set { 
                this.bookLocation = value; 
                this.OnPropertyChanged("BookLocation");
                ((DelegateCommand)this.BookLocationShowClearCommand).RaiseCanExecuteChanged();
            }
        }
        public ICommand BookLocationShowClearCommand
        {
            get {
                if (this.bookLocationShowClearCommand == null)
                {
                    this.bookLocationShowClearCommand = new DelegateCommand(onBookLocationShowClearCommandExecute, onBookLocationShowClearCommandCanExecute);
                }
                return this.bookLocationShowClearCommand;
            }
        }
 
        public DrawMapService LibraryMapService //这个用于UI上的绑定，用于显示两个地图
        {
            get
            {
                return this.libraryMapService;
            }
            set
            {    //由于Canvas是只读的，因此set不做任何具体操作仅仅通知UI界面发生变化
                this.OnPropertyChanged("LibraryMapService");
            }
        }
        private Boolean onBookLocationShowClearCommandCanExecute()
        {
            if (!String.IsNullOrEmpty(this.BookName))
            {
                return true;
            }
            if (!String.IsNullOrEmpty(this.BookAccessCode))
            {
                return true;
            }
            if (!String.IsNullOrEmpty(this.BookLocation))
            {
                return true;
            }
            return false;
        }

        //###########################
        private void onBookLocationShowClearCommandExecute()
        {
            this.clearBookInformation();
        }

        private void handleErrorFromDatabase(string errorMessage)
        {//数据库读操作或者解析失败
            MessageBox.Show(errorMessage);
        }
        private void handleErrorFromRFID(string errorMessage)
        {
            /***
            MessageBox.Show(errorMessage);
            //当串口设置出错时提示信息并转移到串口设置界面
            NavBarViewModel vm = this.container.Resolve<NavBarViewModel>();
            vm.switchSystemSettingView();
             * **/
        }
        private void handleNewItemFromRFID(RFIDContent newItem)
        {
            if (newItem.bookRfidList.Count() != 0)
            {
                IBookLocationService bookLocationService = this.container.Resolve<IBookLocationService>();
                IBookInformationService bookInformationService = this.container.Resolve<IBookInformationService>();
                
                List<String> bookNameList = bookInformationService.getBookNameListByRfidList(newItem.bookRfidList);
                List<String> bookAccessCodeList = bookInformationService.getBookAccessCodeListByRfidList(newItem.bookRfidList);
                this.dispatcherService.Dispatch(() =>
                {//显示图书基本信息
                    this.BookName = bookNameList[0];
                    this.BookAccessCode = bookAccessCodeList[0];
                });               

                String shelfRfid = bookLocationService.getShelfRfidbyBookRfid(newItem.bookRfidList[0]);
                String bookLocationString = bookLocationService.getShelfNameByShelfRfid(shelfRfid);

                //在选定的书架层上绘图
                this.dispatcherService.Dispatch(() =>
                {
                    this.BookLocation = bookLocationString;
                    //test code here ,delete it 
                    this.libraryMapService.reinitOneShapMap();//刷新UI
                    this.libraryMapService.drawOneShapeMapBackground();
                    this.libraryMapService.drawSelectedLayerOneShapeMap(bookLocationString);
                    //this.LibraryMapService = this.LibraryMapService; //通知更新UI，没必要重复了，等下面操作一起刷新

                    this.libraryMapService.reinitLibraryShelfMap();
                    this.libraryMapService.drawLibraryShelfMapBackgroundByLibraryName(bookLocationString);
                    this.libraryMapService.drawSelectedShelfLibraryShelfMapByLibraryName(shelfRfid);
                    this.LibraryMapService = this.LibraryMapService;//通知更新UI

                });
            }
        }
        private void clearBookInformation()
        {
            this.BookName = ""; this.BookAccessCode = ""; this.BookLocation = "";
            this.libraryMapService.reinitOneShapMap();//刷新UI
            this.libraryMapService.drawOneShapeMapBackground();
            //this.LibraryMapService = this.LibraryMapService; //通知更新UI，没必要重复了，等下面操作一起刷新

            this.libraryMapService.reinitLibraryShelfMap();
            this.LibraryMapService = this.LibraryMapService;//通知更新UI
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
