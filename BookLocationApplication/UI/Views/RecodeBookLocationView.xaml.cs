﻿using Microsoft.Practices.Unity;
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
using UI.ViewModels;

namespace UI.Views
{
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
    }
}
