using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestUnit.ViewModel
{
    class TestUserControlViewModelForBookLocation : INotifyPropertyChanged
    {
        IUnityContainer container;
        IBookLocationService bookLocationService;
        String shelfRfidString, bookRfidString;
        public TestUserControlViewModelForBookLocation(IUnityContainer container)
        {
            this.container = container;
            bookLocationService = container.Resolve<IBookLocationService>();
            this.shelfRfidString = ""; this.bookRfidString = "";
        }
        public String ShelfRfidString
        {
            get { return this.shelfRfidString; }
            set { this.shelfRfidString = value; OnPropertyChange("ShelfRfidString"); }
        }
        public String BookRfidString
        {
            get { return this.bookRfidString; }
            set { this.bookRfidString = value; OnPropertyChange("BookRfidString"); }
        }
        public String IP
        {
            get { return bookLocationService.ServerIp; }
            set
            {
                bookLocationService.ServerIp = value;
                OnPropertyChange("IP");
            }
        }
        public String Port
        {
            get { return bookLocationService.ServerPort; }
            set
            {
                bookLocationService.ServerPort = value;
                OnPropertyChange("Port");
            }
        }
        public String Username
        {
            get { return bookLocationService.ServerUsername; }
            set
            {
                bookLocationService.ServerUsername = value;
                OnPropertyChange("Username");
            }
        }
        public String Password
        {
            get { return bookLocationService.ServerPassword; }
            set
            {
                bookLocationService.ServerPassword = value;
                OnPropertyChange("Password");
            }
        }
        public ICommand RecodeBookLocationCommand
        {
            get { return new DelegateCommand(() => { RecodeBookLocation(); }); }
        }
        void RecodeBookLocation()
        {
            String shelfRfid = ShelfRfidString;
            List<String> bookRfidList = BookRfidString.Split(';').ToList<String>();
            if ( (String.IsNullOrEmpty(shelfRfid)) || (bookRfidList.Count == 0) )
            {
                return;
            }
            bookLocationService.setBookRfidListOnShelfRfid(shelfRfid,bookRfidList);
        }
        /// <param name="propertyName"></param>
        public void OnPropertyChange(String propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
