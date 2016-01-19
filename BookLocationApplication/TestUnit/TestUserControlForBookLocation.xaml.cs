using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestUnit.ViewModel;

namespace TestUnit
{
    /// <summary>
    /// Interaction logic for TestUserControlForBookLocation.xaml
    /// </summary>
    public partial class TestUserControlForBookLocation : UserControl
    {
        IUnityContainer container;
        public TestUserControlForBookLocation(IUnityContainer container)
        {
            InitializeComponent();
            this.container = container;
            this.DataContext = container.Resolve<TestUserControlViewModelForBookLocation>();
        }
    }
}
