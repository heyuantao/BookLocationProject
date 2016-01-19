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
        String getShelfNameByShelfRfid(string rfidCode);  //get the display name by shelf rfid code
        String getShelfRfidbyBookRfid(String rfidCode);   //get the shelf rfid code of a book
        Boolean setBookRfidListOnShelfRfid( String shelfRfid,List<String> bookRfidList); //associate the book rfid with a shelf
    
    }
}
