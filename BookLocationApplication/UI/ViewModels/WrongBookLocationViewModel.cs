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
using System.Windows.Input;

namespace UI.ViewModels
{
    public class BookItemOfWrongLocation //这个数据结构用于组成一个ObservableCollection<BookItem>()
    {//用于在ui界面中显示一个列表型的图书信息
        public String ID { get; set; }  //按递增顺序排列
        public String BookName { get; set; }
        public String BookAccessCode { get; set; }
        public String BookRFIDCode { get; set; }
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

        private bool wrongBookLocationCleanAllCommandCanExecute()
        {
            //判断清除按钮是否可以被激活
            if (! String.IsNullOrEmpty(this.shelfLocationString))
            {
                return true;
            }
            if (this.onThisShelfAllBookList.Count() > 0)
            {
                return true;
            }
            if (this.notOnThisShelfBookList.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private void wrongBookLocationCleanAllCommandExecute()
        {
            throw new NotImplementedException();
        }
        //Bind variable and Icommand on UI
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
                this.onThisShelfAllBookList = value; this.OnPropertyChanged("OnThisShelfAllBookList");
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
                this.notOnThisShelfBookList = value; this.OnPropertyChanged("NotOnThisShelfBookList");
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
            //eventAggregator.GetEvent<RFIDNewItemEvent>().Unsubscribe(handleNewItemFromRFID);
            //eventAggregator.GetEvent<RFIDHardwareEvent>().Unsubscribe(handleErrorFromRFID);

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            //ISerialService serialService = container.Resolve<ISerialService>();
            IBookInformationService bookInformationService = container.Resolve<IBookInformationService>();
            IBookLocationService bookLocationService = container.Resolve<IBookLocationService>();
            IRFIDService rfidService = container.Resolve<IRFIDService>();

            //开始读取RFID的信息，并查询数据库
            //eventAggregator.GetEvent<RFIDNewItemEvent>().Subscribe(handleNewItemFromRFID);
            //开始订阅RFID服务发出的事件，这个事件是扫描到的条码的信息，在该view非激活时务必取消此事件的订阅
            //eventAggregator.GetEvent<RFIDHardwareEvent>().Subscribe(handleErrorFromRFID);

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
