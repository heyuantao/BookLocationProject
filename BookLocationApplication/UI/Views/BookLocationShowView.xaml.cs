using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using UI.ViewModels;

namespace UI.Views
{

    /// <summary>
    /// Interaction logic for BookLocationShowView.xaml
    /// </summary>
    public partial class BookLocationShowView : UserControl
    {
        IUnityContainer container;
        public BookLocationShowView(IUnityContainer container, BookLocationShowViewModel viewModel)
        {
            InitializeComponent();
            this.container = container;
            this.DataContext = viewModel;
        }
    }
}
