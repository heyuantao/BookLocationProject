using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Views;

namespace UI
{
    public class UIModule:IModule
    {
        IUnityContainer container;
        IRegionManager regionManager;
        public UIModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }
        public void Initialize()
        {
            container.RegisterType<NavBar, NavBar>();
            container.RegisterType<ShowBookLocation, ShowBookLocation>();
            container.RegisterType<SystemSetting, SystemSetting>();
            regionManager.RegisterViewWithRegion("MainRegion", typeof(SystemSetting));
            regionManager.RegisterViewWithRegion("NavRegion", typeof(NavBar));
        }
    }
}
