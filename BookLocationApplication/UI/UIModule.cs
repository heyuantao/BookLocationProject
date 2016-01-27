using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels;
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
            container.RegisterType<SystemSettingViewModel, SystemSettingViewModel>();
            //SystemSettingViewModel必须先初始化，这样系统配置信息才能载入容器中的对象
            container.RegisterType<NavBarViewModel, NavBarViewModel>();
            container.RegisterType<BookLocationShowViewModel, BookLocationShowViewModel>();
            container.RegisterType<RecodeBookLocationViewModel, RecodeBookLocationViewModel>();

            container.RegisterType<SystemSettingView, SystemSettingView>();
            //SystemSettingViewModel必须先初始化，这样系统配置信息才能载入容器中的对象
            container.RegisterType<NavBarView, NavBarView>();
            container.RegisterType<BookLocationShowView, BookLocationShowView>();
            container.RegisterType<RecodeBookLocationView, RecodeBookLocationView>();
            //container.RegisterInstance<BookLocationShowView>(new BookLocationShowView());
            

            regionManager.RegisterViewWithRegion("NavRegion", typeof(NavBarView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(SystemSettingView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(RecodeBookLocationView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(BookLocationShowView));
        }
    }
}
