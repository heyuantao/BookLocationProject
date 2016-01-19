using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UI.ViewModels
{
    public class NavBarViewModel
    {
        public NavBarViewModel()
        {
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
            int b = 3;
        }

        private void switchBookLocationShowView()
        {
            int a = 3;
        }
    }
}
