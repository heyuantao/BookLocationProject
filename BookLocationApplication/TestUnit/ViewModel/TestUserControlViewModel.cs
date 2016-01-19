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

namespace TestUnit
{
    class TestUserControlViewModel:INotifyPropertyChanged
    {
        IUnityContainer container;
        IBookInformationService bookInformationService;
        String querySource,queryResult;
        public TestUserControlViewModel(IUnityContainer container)
        {
            this.container = container;
            this.bookInformationService = container.Resolve<IBookInformationService>();
            this.querySource = ""; this.queryResult = "";
        }
        public String IP
        {
            get { return bookInformationService.ServerIp; }
            set {
                bookInformationService.ServerIp = value;
                OnPropertyChange("IP");
            }
        }
        public String Port
        {
            get { return bookInformationService.ServerPort; }
            set
            {
                bookInformationService.ServerPort = value;
                OnPropertyChange("Port");
            }
        }
        public String Username
        {
            get { return bookInformationService.ServerUsername; }
            set
            {
                bookInformationService.ServerUsername = value;
                OnPropertyChange("Username");
            }
        }
        public String Password
        {
            get { return bookInformationService.ServerPassword; }
            set
            {
                bookInformationService.ServerPassword = value;
                OnPropertyChange("Password");
            }
        }
        public String QuerySource
        {
            get { return this.querySource; }
            set { this.querySource = value; OnPropertyChange("QuerySource"); }
        }
        public String QueryResult
        {
            get { return this.queryResult; }
            set { this.queryResult = value; OnPropertyChange("QueryResult"); }
        }
        public ICommand saveCommand
        {
            get { return new DelegateCommand(() => { IP="sqlserver.syslab.org"; }); }
        }
        public ICommand queryCommand
        {
            get { return new DelegateCommand(() => { this.QueryResult = ""; query(); }); }
        }
        public ICommand clearCommand
        {
            get { return new DelegateCommand(()=>{this.QueryResult="";}); }
        }
        void query()
        {
            List<String> rfidCodeList = new List<string>();
            List<String> bookNameList;
            String finalString = "";
            String[] content = this.QuerySource.Split(';');

            foreach (String code in content)
            {
                rfidCodeList.Add(code);
            }

            bookNameList = (List<string>)bookInformationService.getBookNameListByRfidList(rfidCodeList);
            foreach(String item in bookNameList){
                finalString=finalString+"\n"+item;
            }
            this.QueryResult = finalString;
        }
        /// <summary>
        /// for super class
        /// </summary>
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
