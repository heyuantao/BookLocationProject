using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Views;

namespace UI.ViewModels
{
    public class NavBarViewModel
    {
        IUnityContainer container;
        IRegionManager regionManager;
        public NavBarViewModel(IUnityContainer container,IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
        }
        public ICommand BookLocationShowViewICommand
        {
            get { return new DelegateCommand(switchBookLocationShowView); }
        }
        public ICommand SystemSettingViewICommand
        {
            get { return new DelegateCommand(switchSystemSettingView); }
        }

        private void switchSystemSettingView()
        {
            regionManager.RequestNavigate("MainRegion", new Uri("SystemSettingView", UriKind.Relative));
        }

        private void switchBookLocationShowView()
        {
            //regionManager.Regions["MainRegion"].Deactivate;
            regionManager.RequestNavigate("MainRegion", new Uri("BookLocationShowView", UriKind.Relative));
            //regionManager.RegisterViewWithRegion("MainRegion", ()=>container.Resolve<BookLocationShowView>());
        }
    }
}
