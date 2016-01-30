using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInformation
{
    public class MiaoSiBookInformationService:IBookInformationService
    {
        IUnityContainer container;
        IEventAggregator eventAggregator;
        String serverIp, serverPort, serverUsername, serverPassword;
        String databaseConnectionString;
        MiaoSiBookDatabase database;
        public MiaoSiBookInformationService(IUnityContainer container)
        {
            this.container = container;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            this.serverIp = "sqlserver.syslab.org"; this.serverPort = "0";this.serverUsername="sa";this.serverPassword="19831122";
            this.databaseConnectionString = "";
            this.databaseConnectionString = generateDatabaseConnectionString();
            this.database = new MiaoSiBookDatabase(databaseConnectionString);
        }
        public String ServerIp
        {
            get { return this.serverIp; }
            set { this.serverIp = value; this.databaseConnectionString = generateDatabaseConnectionString(); }
        }
        public String ServerPort
        {
            get { return this.serverPort; }
            set { this.serverPort = value; this.databaseConnectionString = generateDatabaseConnectionString(); ; }
        }
        public String ServerUsername
        {
            get { return this.serverUsername; }
            set { this.serverUsername = value; this.databaseConnectionString = generateDatabaseConnectionString(); ; }
        }
        public String ServerPassword
        {
            get { return this.serverPassword; }
            set { this.serverPassword = value; this.databaseConnectionString = generateDatabaseConnectionString(); ; }
        }
        public List<String> getBookAccessCodeListByRfidList(List<String> rfidList)
        {
            List<String> BookAccessCodeList = new List<string>();
            try
            {
                foreach (String rfidCode in rfidList)
                {
                    String accessCode = database.getBookAccessCodeByDecodedRFID(rfidCode);
                    BookAccessCodeList.Add(accessCode);
                }
            }
            catch (DataBaseConnectException)
            {
                eventAggregator.GetEvent<DatabaseEvent>().Publish("秒思数据库连接错误!");
            }
            catch (DataBaseQueryException)
            {
                eventAggregator.GetEvent<DatabaseEvent>().Publish("秒思数据库查询错误!");
            }
            return BookAccessCodeList;
        }
        public List<String> getBookNameListByRfidList(List<String> rfidList)
        {
            List<String> BookNameList = new List<String>();
            try
            {
                foreach (String rfidCode in rfidList)
                {
                    String bookName = database.getBookNameByDecodedRFID(rfidCode);
                    BookNameList.Add(bookName);
                }
            }
            catch (DataBaseConnectException)
            {
                eventAggregator.GetEvent<DatabaseEvent>().Publish("秒思数据库连接错误!");
            }
            catch (DataBaseQueryException)
            {
                eventAggregator.GetEvent<DatabaseEvent>().Publish("秒思数据库查询错误!");
            }
            return BookNameList;
        }
        String generateDatabaseConnectionString(){
            string ip = this.ServerIp;
            string username = this.ServerUsername;
            string password = this.ServerPassword;
            //Data Source=192.168.28.129;Initial Catalog=Dblibrary;Persist Security Info=True;User ID=sa;Password=19831122
            String conString = "Data Source=" + ip + ";Initial Catalog=Dblibrary;Persist Security Info=True;User ID=" + username + ";Password=" + password;
            this.database = new MiaoSiBookDatabase(conString);
            return conString;
        }
    }
}
