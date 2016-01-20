using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels
{
    public class BookLocationShowViewModel
    {
        IUnityContainer container;
        IRegionManager regionManager;
        public BookLocationShowViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
        }

    }
}
