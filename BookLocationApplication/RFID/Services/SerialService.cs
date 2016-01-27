using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID
{
    public class SerialService : ISerialService
    {
        IUnityContainer container;
        IEventAggregator eventAggregator;
        String serial; //serial interface name such as "COM1" "COM2"
        String speed;  //speed of serial , such as 115200,9600
        public SerialService(IUnityContainer container)
        {
            this.container = container;
            this.eventAggregator = container.Resolve<IEventAggregator>();
            this.serial = "";
            this.speed = "";
        }
        public String Serial
        {
            get { return this.serial; }
            set { this.serial = value; }
        }
        public String Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }
        public List<String> SerialList()
        {
            string[] PortListInString;
            List<String> PortList = new List<String>();
            try
            {
                PortListInString = SerialPort.GetPortNames();
                foreach (String item in PortListInString)
                {
                    PortList.Add(item);
                }
                return PortList;
            }
            catch (Exception)
            {
                eventAggregator.GetEvent<RFIDHardwareEvent>().Publish("获取串口列表失败！");
                return new List<String>();
            }
        }
    }
}
