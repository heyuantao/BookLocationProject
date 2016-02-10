using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IBookLocationService
    {
        String ServerIp { get; set; }
        String ServerPort { get; set; }
        String ServerUsername { get; set; }
        String ServerPassword { get; set; }
        // for Shelf And BookOnShelf table
        String getShelfNameByShelfRfid(string rfidCode);  //get the display name by shelf rfid code
        String getShelfRfidbyBookRfid(String rfidCode);   //get the shelf rfid code of a book
        Boolean setBookRfidListOnShelfRfid( String shelfRfid,List<String> bookRfidList); //associate the book rfid with a shelf
        List<String> getBookRfidListOnShelfRfid(String shelfRfid);//从书架RFID查询登记在该书架上的所有图书
        ///for Map table
        ///getItemPositionListByLocationAndType 返回的是没有重复值的
        List<String> getItemPositionStringListByLocationAndType(string location, string type);
        String getItemPositionStringByShelfRfid(string shelfRfid);
    }
}
