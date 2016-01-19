using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInformation
{
    public class BookInformationModule : IModule
    {
        IUnityContainer container;
        public BookInformationModule(IUnityContainer container)
        {
            this.container = container;
        }
        public void Initialize()
        {
            MiaoSiBookInformationService bookLocationService = new MiaoSiBookInformationService(container);
            container.RegisterInstance<IBookInformationService>(bookLocationService);

        }
    }
}
