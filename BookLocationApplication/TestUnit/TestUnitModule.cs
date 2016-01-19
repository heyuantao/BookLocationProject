using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUnit.ViewModel;

namespace TestUnit
{
    public class TestUnitModule:IModule
    {
        IUnityContainer container;
        IRegionManager regionManager;
        public TestUnitModule(IUnityContainer container,IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }
        public void Initialize() 
        {
            this.unitTestForBookLocationService();  //unit test for BookLocationService
        }
        private void unitTestForBookLocationService()
        {
            //this is unit test code for booklocation code
            container.RegisterType<TestUserControl, TestUserControl>();
            container.RegisterType<TestUserControlViewModel, TestUserControlViewModel>();
            container.RegisterType<TestUserControlForBookLocation, TestUserControlForBookLocation>();
            container.RegisterType<TestUserControlViewModelForBookLocation, TestUserControlViewModelForBookLocation>();
            regionManager.RegisterViewWithRegion("MainRegion", typeof(TestUserControl));
            regionManager.RegisterViewWithRegion("OtherRegion", typeof(TestUserControlForBookLocation));
            
        }
    }
}
