using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels
{
    public class SystemSettingViewModel
    {
        IUnityContainer container;
        IRegionManager regionManager;
        public SystemSettingViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
        }
    }
}
