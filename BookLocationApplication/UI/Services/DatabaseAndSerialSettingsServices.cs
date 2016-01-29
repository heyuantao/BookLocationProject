using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels;

namespace UI.Services
{
    //这个服务不需要注入容器
    public class DatabaseAndSerialSettingsServices
    {
        BookInformationServerSettings bookInformationServerSettings;
        BookLocationServerSettings bookLocationServerSettings;
        SerialSettings serialSettings;
        public DatabaseAndSerialSettingsServices()
        {
            bookInformationServerSettings = new BookInformationServerSettings();
            bookLocationServerSettings = new BookLocationServerSettings();
            serialSettings = new SerialSettings();
        }
        public BookInformationServerSettings loadBookInformationServerSettings()
        {
            this.bookInformationServerSettings.IP = Properties.CustomSettings.Default.bookInformationServerIP;
            this.bookInformationServerSettings.Username = Properties.CustomSettings.Default.bookInformationServerUsername;
            this.bookInformationServerSettings.Password = Properties.CustomSettings.Default.bookInformationServerPassword;
            return this.bookInformationServerSettings;
        }
        public BookLocationServerSettings loadBookLocationServerSettings()
        {
            this.bookLocationServerSettings.IP = Properties.CustomSettings.Default.bookLocationServerIP;
            this.bookLocationServerSettings.Username = Properties.CustomSettings.Default.bookLocationServerUsername;
            this.bookLocationServerSettings.Password = Properties.CustomSettings.Default.bookLocationServerPassword;
            return this.bookLocationServerSettings;
        }
        public SerialSettings loadSerialSettings()
        {
            this.serialSettings.Serial = Properties.CustomSettings.Default.serialName;
            this.serialSettings.Speed = Properties.CustomSettings.Default.serialSpeed;
            return this.serialSettings;
        }
        public void saveBookInformationServerSettings(BookInformationServerSettings settings){
            Properties.CustomSettings.Default.bookInformationServerIP = settings.IP;
            Properties.CustomSettings.Default.bookInformationServerUsername = settings.Username;
            Properties.CustomSettings.Default.bookInformationServerPassword = settings.Password;
            Properties.CustomSettings.Default.Save();
        }
        public void saveBookLocationServerSettings(BookLocationServerSettings settings)
        {
            Properties.CustomSettings.Default.bookLocationServerIP = settings.IP;
            Properties.CustomSettings.Default.bookLocationServerUsername = settings.Username;
            Properties.CustomSettings.Default.bookLocationServerPassword = settings.Password;
            Properties.CustomSettings.Default.Save();
        }
        public void saveSerialSettings(SerialSettings settings)
        {
            Properties.CustomSettings.Default.serialName = settings.Serial;
            Properties.CustomSettings.Default.serialSpeed = settings.Speed;
            Properties.CustomSettings.Default.Save();
        }

    }
}
