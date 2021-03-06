﻿using Microsoft.Practices.Unity;
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
    public class MyConverter : IValueConverter
    {

        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            return (double)value - 80;
        }

        public object ConvertBack(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            return (double)value + 80;
        }

    }
    /// <summary>
    /// Interaction logic for RecodeBookLocationView.xaml
    /// </summary>
    public partial class RecodeBookLocationView : UserControl
    {
        IUnityContainer container;
        public RecodeBookLocationView(IUnityContainer container,RecodeBookLocationViewModel viewModel)
        {
            InitializeComponent();
            this.container = container;
            this.DataContext = viewModel;
        }
        //用这个方法设定高度违背了设计原则，但实在找不到更合适的高度设置方式
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double height = Application.Current.MainWindow.ActualHeight;
            this.tableDataGrid.Height = height - 200;
        }
    }
}
