﻿using Infrastructure;
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
using System.Windows.Input;

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
        public BookLocationShowViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            this.dispatcherService = container.Resolve<IDispatcherService>();
            //初始化UI的变量
            this.bookName = ""; this.bookAccessCode = ""; this.bookLocation = "";
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

            rfidService.start();//开启rfid的后台扫描线程
        }
        public String BookName
        {
            get { return this.bookName; }
            set { this.bookName = value; this.OnPropertyChanged("BookName"); }
        }
        public String BookAccessCode
        {
            get { return this.bookAccessCode; }
            set { this.bookAccessCode = value; this.OnPropertyChanged("BookAccessCode"); }
        }
        public String BookLocation
        {
            get { return this.bookLocation; }
            set { this.bookLocation = value; this.OnPropertyChanged("BookLocation"); }
        }
        public ICommand BookLocationShowClearCommand
        {
            get { return new DelegateCommand(onBookLocationShowClearCommandExecute); }
        }

        //###########################
        private void onBookLocationShowClearCommandExecute()
        {
            this.clearBookInformation();
        }
        private void handleErrorFromRFID(string errorMessage)
        {
            MessageBox.Show(errorMessage);
            //当串口设置出错时提示信息并转移到串口设置界面
            this.regionManager.RequestNavigate("MainRegion", new Uri("SystemSettingView", UriKind.Relative));

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
                {
                    this.BookName = bookNameList[0];
                    this.BookAccessCode = bookAccessCodeList[0];
                });               

                String shelfRfid = bookLocationService.getShelfRfidbyBookRfid(newItem.bookRfidList[0]);
                this.dispatcherService.Dispatch(() =>
                {
                    this.BookLocation = bookLocationService.getShelfNameByShelfRfid(shelfRfid);
                });   
            }
        }
        private void clearBookInformation()
        {
            this.BookName = ""; this.BookAccessCode = ""; this.BookLocation = "";
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
