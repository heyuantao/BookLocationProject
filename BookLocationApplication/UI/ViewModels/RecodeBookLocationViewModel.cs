using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels
{
    public class RecodeBookLocationViewModel : INavigationAware
    {
        IUnityContainer container;
        IRegionManager regionManager;
        public RecodeBookLocationViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
            ISerialService serialService = container.Resolve<ISerialService>();
            //开始编码
            //String com = serialService.Serial;
            //String comSpeed = serialService.Speed;
        }
    }
}
