using Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UnityContainer container = new UnityContainer();
            container.RegisterInstance<IServiceLocator>(new UnityServiceLocatorAdapter(container));

            container.RegisterType<IModuleInitializer, ModuleInitializer>();

            TextLogger logger = new TextLogger();
            container.RegisterInstance<ILoggerFacade>(logger);

            IEventAggregator eventAggregator = new EventAggregator();
            container.RegisterInstance<IEventAggregator>(eventAggregator);

            ModuleCatalog catalog = new ModuleCatalog();
            catalog.AddModule(typeof(RFID.RFIDModule));
            container.RegisterInstance<IModuleCatalog>(catalog);

            container.RegisterType<IModuleManager, ModuleManager>();
            IModuleManager manager = container.Resolve<IModuleManager>();
            manager.Run();
            //测试RFID服务模块的功能
            RFIDUnitTest.testRFID(container);
            /******/
            //测试串口服务的功能
            //SerialUnitTest.testSerial(container);
        }

        
    }
}
