using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class RFIDContent
    {
        public List<String> bookRfidList;
        public List<String> shelfRfidList;
        public RFIDContent()
        {
            this.bookRfidList = new List<String>();
            this.shelfRfidList = new List<String>();
        }
    }
    public class RFIDNewItemEvent : PubSubEvent<RFIDContent>
    {
        //this event happen with the RFIDContent
        //ths content fill what was read by rfid reader
    }
}
