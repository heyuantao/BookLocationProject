using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLocation
{
    public class BookLocationModule:IModule
    {
        IUnityContainer container;
        public BookLocationModule(IUnityContainer container)
        {
            this.container = container;
        }
        public void Initialize()
        {
            //MiaoSiBookLocationService bookLocationService = new MiaoSiBookLocationService(container);
            //container.RegisterInstance<IBookLocationService>(bookLocationService);
            BookLocationService bookLocationService = new BookLocationService(container);
            container.RegisterInstance<IBookLocationService>(bookLocationService);
        }
    }
}
