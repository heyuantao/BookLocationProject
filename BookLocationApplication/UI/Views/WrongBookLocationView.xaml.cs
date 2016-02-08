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

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for WrongBookLocationView.xaml
    /// </summary>
    public partial class WrongBookLocationView : UserControl
    {
        IUnityContainer container;
        public WrongBookLocationView(IUnityContainer container)
        {
            InitializeComponent();
            this.container = container;
        }
        //用这个方法设定高度违背了设计原则，但实在找不到更合适的高度设置方式
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double height = Application.Current.MainWindow.ActualHeight;
            //this.tableDataGrid.Height = height - 200;
            double margin = 300; //除了两个grid之外预留的位置
            this.allBookOnShelfDataGrid.Height = (height - margin) / 2;
            this.notOnShlefBookDataGrid.Height = (height - margin) / 2;
        }
    }
}
