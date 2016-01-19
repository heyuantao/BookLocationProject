using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest
{
    class RFIDUnitTest
    {
        public static void testRFID(UnityContainer container)  //测试程序入口
        {
            IEventAggregator eventAggregator = container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<RFIDHardwareEvent>().Subscribe(RFIDFailureHander);
            eventAggregator.GetEvent<RFIDNewItemEvent>().Subscribe(RFIDNewItemHander);
            IRFIDService rfidService = container.Resolve<IRFIDService>();
            rfidService.HardwareInterface = "";
            rfidService.HardwareInterfaceConnectionSpeed = "";
            rfidService.start();
            readInputAndChangeStatus(container);
            Console.ReadLine();
        }

        private static void readInputAndChangeStatus(UnityContainer container)
        {
            IRFIDService rfidService = container.Resolve<IRFIDService>();
            String command = "";
            while (true)
            {
                Console.Write("Input a command:");
                command = Console.ReadLine();
                if (command == "start")
                {
                    rfidService.start();
                    Console.WriteLine("Start RFID！");
                }
                if (command == "stop")
                {
                    rfidService.stop();
                    Console.WriteLine("Stop RFID！");
                }
                if (command.StartsWith("set"))
                {
                    String[] strArray=command.Split(' ');
                    if (strArray.Length != 3)
                    {
                        continue;
                    }
                    rfidService.HardwareInterface = strArray[1];
                    rfidService.HardwareInterfaceConnectionSpeed = strArray[2];
                }
            }
        }

        private static void RFIDFailureHander(String msg)
        {
            Console.WriteLine("This is msg in hander !");
            Console.WriteLine(msg);
        }
        private static void RFIDNewItemHander(RFIDContent obj)
        {
            if ((obj.bookRfidList.Count == 0) && (obj.shelfRfidList.Count == 0))
            {
                return;
            }
            Console.WriteLine("new item");
            Console.Write("book:");
            foreach (String item in obj.bookRfidList)
            {
                Console.Write(item + "  ");
            }
            Console.WriteLine();
            Console.Write("shelf:");
            foreach (String item in obj.shelfRfidList)
            {
                Console.Write(item + "  ");
            }
            Console.WriteLine("end");
        }

    }
}
