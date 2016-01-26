using Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest
{
    class SerialUnitTest
    {
        public static void testSerial(IUnityContainer container)
        {
            ISerialService serialService = container.Resolve<ISerialService>();
            IList<String> serialList = serialService.SerialList();
            Console.WriteLine("This is current serial list:");
            foreach (String item in serialList)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
