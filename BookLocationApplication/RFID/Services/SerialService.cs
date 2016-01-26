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
        public SerialService(IUnityContainer container)
        {
            this.container = container;
            this.eventAggregator = container.Resolve<IEventAggregator>();

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
