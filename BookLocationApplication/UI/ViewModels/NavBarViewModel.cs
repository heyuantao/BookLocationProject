using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Views;

namespace UI.ViewModels
{
    public class NavBarViewModel
    {
        IUnityContainer container;
        IRegionManager regionManager;
        ICommand bookLocationShowViewICommand;
        ICommand recodeBookLocationViewICommand;
        ICommand wrongBookLocationViewICommand;
        ICommand systemSettingViewICommand;
        //用于存储当前在MainRegion中显示出的View的名称
        String currentViewNameInMainRegion;
        public NavBarViewModel(IUnityContainer container,IRegionManager regionManager)
        {
            this.container = container; this.regionManager = regionManager;
            //初始化ICommand
            this.bookLocationShowViewICommand = new DelegateCommand(switchBookLocationShowView, canSwitchBookLocationShowView);
            this.recodeBookLocationViewICommand=new DelegateCommand(switchRecodeBookLocationView,canSwitchRecodeBookLocationView);
            this.wrongBookLocationViewICommand=new DelegateCommand(switchWrongBookLocationView,canSwitchWrongBookLocationView);
            this.systemSettingViewICommand = new DelegateCommand(switchSystemSettingView, canSwitchSystemSettingView);
            //初始化本控件所需要的变量
            this.currentViewNameInMainRegion = "";
        }


        public ICommand BookLocationShowViewICommand
        {
            get { return this.bookLocationShowViewICommand; }
        }
        public ICommand RecodeBookLocationViewICommand
        {
            get { return this.recodeBookLocationViewICommand; }
        }
        public ICommand WrongBookLocationViewICommand
        {
            get { return this.wrongBookLocationViewICommand; }
        }
        public ICommand SystemSettingViewICommand
        {
            get { return this.systemSettingViewICommand; }
        }
        //页面切换功能，对currentViewNameInMainRegion的赋值必须在regionManager.RequestNavigate
        //的前面，因为regionManager.RequestNavigate的执行可能会改变currentViewNameInMainRegion
        //的值，从而使程序产生不可预知的结果
        public void switchRecodeBookLocationView()
        {
            this.currentViewNameInMainRegion = "RecodeBookLocationView";
            updateNavigationButtonStatus();
            regionManager.RequestNavigate("MainRegion", new Uri("RecodeBookLocationView", UriKind.Relative));
            updateNavigationButtonStatus();
        }
        public void switchWrongBookLocationView()
        {
            this.currentViewNameInMainRegion = "WrongBookLocationView";
            updateNavigationButtonStatus();
            regionManager.RequestNavigate("MainRegion", new Uri("WrongBookLocationView", UriKind.Relative));
            updateNavigationButtonStatus();
        }
        public void switchSystemSettingView()
        {
            this.currentViewNameInMainRegion = "SystemSettingView";
            updateNavigationButtonStatus();
            regionManager.RequestNavigate("MainRegion", new Uri("SystemSettingView", UriKind.Relative));
            updateNavigationButtonStatus();
        }
        public void switchBookLocationShowView()
        {
            /***
            BookLocationShowView view = container.Resolve<BookLocationShowView>();
            IRegion mainRegion = regionManager.Regions["MainRegion"];
            mainRegion.Add(view);
             * **/
            //regionManager.Regions["MainRegion"].Deactivate;
            this.currentViewNameInMainRegion = "BookLocationShowView";
            updateNavigationButtonStatus();
            regionManager.RequestNavigate("MainRegion", new Uri("BookLocationShowView", UriKind.Relative));
            updateNavigationButtonStatus();
            //regionManager.RegisterViewWithRegion("MainRegion", ()=>container.Resolve<BookLocationShowView>());
        }

        //本控件的业务逻辑
        private void updateNavigationButtonStatus()
        {
            ((DelegateCommand)this.BookLocationShowViewICommand).RaiseCanExecuteChanged();
            ((DelegateCommand)this.RecodeBookLocationViewICommand).RaiseCanExecuteChanged();
            ((DelegateCommand)this.WrongBookLocationViewICommand).RaiseCanExecuteChanged();
            ((DelegateCommand)this.SystemSettingViewICommand).RaiseCanExecuteChanged();        
        }
        //判断导航按钮的激活状态
        private bool canSwitchSystemSettingView()
        {
            if (this.currentViewNameInMainRegion != "SystemSettingView")
            {
                return true;
            }
            return false;
        }
        private bool canSwitchWrongBookLocationView()
        {
            if (this.currentViewNameInMainRegion != "WrongBookLocationView")
            {
                return true;
            }
            return false;
        }
        private bool canSwitchRecodeBookLocationView()
        {
            if (this.currentViewNameInMainRegion != "RecodeBookLocationView")
            {
                return true;
            }
            return false;
        }
        private bool canSwitchBookLocationShowView()
        {
            if (this.currentViewNameInMainRegion != "BookLocationShowView")
            {
                return true;
            }
            return false;
        }
    }
}
