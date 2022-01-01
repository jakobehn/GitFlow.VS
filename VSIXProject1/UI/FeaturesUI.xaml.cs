﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GitFlowVS.Extension.ViewModels;
using Microsoft.VisualStudio.Shell;

namespace GitFlowVS.Extension.UI
{
    /// <summary>
    /// Interaction logic for FeaturesUI.xaml
    /// </summary>
    public partial class FeaturesUI : UserControl
    {
        public FeaturesUI(FeaturesViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void FeaturesGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var model = (FeaturesViewModel)DataContext;
            model.CheckoutFeatureBranch();
        }
    }
}
