using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID
{
    public class RFIDModule:IModule
    {
        IUnityContainer container;
        public RFIDModule(IUnityContainer container)
        {
            this.container = container;
        }
        public void Initialize()
        {
            RFIDService rfidService = new RFIDService(container);
            container.RegisterInstance<IRFIDService>(rfidService);

            SerialService serialService = new SerialService(container);
            container.RegisterInstance<ISerialService>(serialService);
        }
    }
}
