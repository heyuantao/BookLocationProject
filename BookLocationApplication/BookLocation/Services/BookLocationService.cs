using Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLocation.Models;
using System.Data.Entity.Core.EntityClient;

namespace BookLocation
{
    public class BookLocationService:IBookLocationService
    {
        IUnityContainer container;
        String entitiesConnectionString;
        LocationEntities dbEntities;
        String serverIp, serverPort, serverUsername, serverPassword;
        public BookLocationService(IUnityContainer container)
        {
            this.container = container;

            this.ServerIp = "sqlserver.syslab.org"; this.ServerPort = "";
            this.ServerUsername="sa";this.ServerPassword="19831122";
            generateEntities();
            //dbEntities = new LocationEntities(this.entitiesConnectionString);
        }
        public String ServerIp
        {
            get { return this.serverIp; }
            set { this.serverIp = value; generateEntities(); }
        }
        public String ServerPort
        {
            get { return this.serverPort; }
            set { this.serverPort = value; generateEntities(); }
        }
        public String ServerUsername
        {
            get { return this.serverUsername; }
            set { this.serverUsername = value; generateEntities(); }
        }
        public String ServerPassword
        {
            get { return this.serverPassword; }
            set { this.serverPassword = value; generateEntities(); }
        }
        void generateEntities()  //use connection param to modify the connection string and dbcontext;
        {  //to continue
            string ip = this.ServerIp;
            string username = this.ServerUsername;
            string password = this.ServerPassword;
            //Data Source=192.168.28.129;Initial Catalog=Dblibrary;Persist Security Info=True;User ID=sa;Password=19831122
            String sqlConnectionString = "Data Source=" + ServerIp + ";Initial Catalog=Location;Persist Security Info=True;User ID=" + ServerUsername + ";Password=" + ServerPassword;
            EntityConnectionStringBuilder eb = new EntityConnectionStringBuilder();
            eb.Metadata = "res://*/Models.Location.csdl|res://*/Models.Location.ssdl|res://*/Models.Location.msl";
            eb.Provider = "System.Data.SqlClient";
            eb.ProviderConnectionString = sqlConnectionString;
            this.entitiesConnectionString=eb.ToString();
            this.dbEntities = new LocationEntities(this.entitiesConnectionString);
        }
        // for Shelf And BookOnShelf table
        public String getShelfNameByShelfRfid(string rfidCode)
        {
            if (String.IsNullOrEmpty(rfidCode))
            {
                return "";
            }
            Shelf oneShelf = dbEntities.Shelf.Where((item) => item.rfidCode == rfidCode).FirstOrDefault<Shelf>();
            if (oneShelf == null)
            {
                return "";
            }
            else
            {
                String name = "";
                name = name + "图书馆" + oneShelf.selection.Trim() + "区";
                name = name + "" + oneShelf.floor + "层";
                name = name + "" + oneShelf.row + "行";
                name = name + "" + oneShelf.col + "列";
                name = name + "" + oneShelf.side.Trim() + "面";
                name = name + "书架第" + oneShelf.shelfFloor + "层";
                return name;
            }            
        }
        public String getShelfRfidbyBookRfid(String rfidCode)
        {
            if (String.IsNullOrEmpty(rfidCode))
            {
                return "";
            }
            BookOnShelf obj = dbEntities.BookOnShelf.Where((item) => item.bookRfidCode == rfidCode).FirstOrDefault<BookOnShelf>();
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.shelfRfidCode;
            }            
        }
        public Boolean setBookRfidListOnShelfRfid(String shelfRfid,List<String> bookRfidList)
        {
            if (bookRfidList.Count == 0||String.IsNullOrEmpty(shelfRfid))
            {
                return false;
            }
            //delete the item in book on shelf
            foreach (String bookRfidCode in bookRfidList)
            {
                var findItemList = dbEntities.BookOnShelf.Where((item) => item.bookRfidCode == bookRfidCode);
                foreach (BookOnShelf item in findItemList)
                {
                    dbEntities.BookOnShelf.Remove(item);
                }
            }
            //insert the item in book on shelf
            foreach (String bookRfidCode in bookRfidList)
            {               
                BookOnShelf item = new BookOnShelf();
                item.shelfRfidCode = shelfRfid;
                item.bookRfidCode = bookRfidCode;
                dbEntities.BookOnShelf.Add(item);
            }
            dbEntities.SaveChanges();
            return true;
        }
        ///for Map table
        ///getItemPositionListByLocationAndType 返回的是没有重复值的
        List<String> getItemPositionStringListByLocationAndType(string location, string type)
        {
            var findItemList=dbEntities.Map.Where((item) => item.location == location);
            foreach (Map item in findItemList)
            {
                //tobe continue
            }
            return new List<String>();
        }
        String getItemPositionStringByShelfRfid(string shelfRfid)
        {
            return "";
        }
    }
}
