using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Services;

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
        List<String> speedList;
        public SerialSettings()
        {
            this.speedList = new List<String>();
            this.speedList.Add("9600"); speedList.Add("115200");
            this.Speed = "115200";
        }
        public String Serial { get; set; }
        public String Speed { get; set; }
        public List<String> SpeedList { get { return this.speedList; } }
    }
    public class SystemSettingViewModel : INotifyPropertyChanged  
    {
        IUnityContainer container;
        IRegionManager regionManager;
        BookInformationServerSettings bookInformationServerSettings;
        BookLocationServerSettings bookLocationServerSettings;
        SerialSettings serialSettings;
        DatabaseAndSerialSettingsServices databaseAndSerialSettingsServices;
        //管理配置文件的类
        public SystemSettingViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
            //初始化时从配置文件中载入基本配置，并在点击保存按钮时把配置保存下来
            this.databaseAndSerialSettingsServices = new DatabaseAndSerialSettingsServices();
            this.bookInformationServerSettings = databaseAndSerialSettingsServices.loadBookInformationServerSettings();
            this.bookLocationServerSettings = databaseAndSerialSettingsServices.loadBookLocationServerSettings();
            this.serialSettings = databaseAndSerialSettingsServices.loadSerialSettings();
            //把配置信息放到容器中的相应对象中,因此这部分必须要比其他的viewModel和view首先被初始化
            //此后容器里的两个数据库服务，和一个RFID服务就有了配置信息
            IBookInformationService bookInformationSrv = this.container.Resolve<IBookInformationService>();
            bookInformationSrv.ServerIp = BookInformationServer.IP;
            bookInformationSrv.ServerUsername = BookInformationServer.Username;
            bookInformationSrv.ServerPassword = BookInformationServer.Password;
            IBookLocationService bookLocationSrv = container.Resolve<IBookLocationService>();
            bookLocationSrv.ServerIp = BookLocationServer.IP;
            bookLocationSrv.ServerUsername = BookLocationServer.Username;
            bookLocationSrv.ServerPassword = BookLocationServer.Password;
            ISerialService serialSrv = container.Resolve<ISerialService>();
            serialSrv.Serial = Serial.Serial;
            serialSrv.Speed = Serial.Speed;

        }
        public BookInformationServerSettings BookInformationServer
        {//用于绑定的属性
            get { return this.bookInformationServerSettings; }
            set {
                this.bookInformationServerSettings = value;
                OnPropertyChanged("BookInformationServer");
            }
        }
        public ICommand BookInformationServerSaveCommand
        {
            get { return new DelegateCommand(onBookInformationServerSaveCommandExecute); }
        }

        private void onBookInformationServerSaveCommandExecute()
        {
            //throw new NotImplementedException();
            //保持该设置到系统的配置文件中
            //BookInformationServerSettings info = this.bookInformationServerSettings;
            IBookInformationService service = this.container.Resolve<IBookInformationService>();
            service.ServerIp = BookInformationServer.IP;
            service.ServerUsername = BookInformationServer.Username;
            service.ServerPassword = BookInformationServer.Password;
            //保存配置文件
            databaseAndSerialSettingsServices.saveBookInformationServerSettings(BookInformationServer);
        }
        public BookLocationServerSettings BookLocationServer
        {//用于绑定的属性
            get { return this.bookLocationServerSettings; }
            set
            {
                this.bookLocationServerSettings = value;
                OnPropertyChanged("BookLocationServer");
            }
        }
        public ICommand BookLocationServerSaveCommand
        {
            get { return new DelegateCommand(onBookLocationServerSaveCommandExecute); }
        }

        private void onBookLocationServerSaveCommandExecute()
        {
            IBookLocationService service = container.Resolve<IBookLocationService>();
            service.ServerIp = BookLocationServer.IP;
            service.ServerUsername = BookLocationServer.Username;
            service.ServerPassword = BookLocationServer.Password;
            //保存配置
            databaseAndSerialSettingsServices.saveBookLocationServerSettings(BookLocationServer);
        }
        public SerialSettings Serial
        {//用于绑定的属性
            get { return this.serialSettings; }
            set
            {
                this.serialSettings = value;
                OnPropertyChanged("Serial");
            }
        }
        public ICommand SerialSaveCommand
        {
            get { return new DelegateCommand(onSerialSaveCommandExecute); }
        }

        private void onSerialSaveCommandExecute()
        {
            ISerialService service = container.Resolve<ISerialService>();
            service.Serial = Serial.Serial;
            service.Speed = Serial.Speed;
            //保存配置
            databaseAndSerialSettingsServices.saveSerialSettings(Serial);
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
