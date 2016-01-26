using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels
{
    public class BookInformationServerSettings
    {
        public String IP { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }
    public class BookLocationServerSettings
    {
        public String IP { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
    }
    public class SerialSettings
    {
        public String Serial { get; set; }
        public String Speed { get; set; }
    }
    public class SystemSettingViewModel : INotifyPropertyChanged  
    {
        IUnityContainer container;
        IRegionManager regionManager;
        BookInformationServerSettings bookInformationServerSettings;
        BookLocationServerSettings bookLocationServerSettings;
        SerialSettings serialSettings;

        public SystemSettingViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
            this.bookInformationServerSettings = new BookInformationServerSettings(){IP="localhost",Username="",Password=""};
            this.bookLocationServerSettings = new BookLocationServerSettings(){IP="127.0.0.1",Username="",Password=""};
            this.serialSettings = new SerialSettings(){Serial="COM1",Speed="115200"};
        }
        public BookInformationServerSettings BookInformationServer
        {
            get { return this.bookInformationServerSettings; }
            set {
                this.bookInformationServerSettings = value;
                OnPropertyChanged("BookInformationServer");
            }
        }

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
