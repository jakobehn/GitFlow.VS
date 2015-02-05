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
using GitFlowVS.Extension.ViewModels;

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

            FeaturesGrid.Loaded += FeaturesGridOnLoaded;
        }

        private void FeaturesGridOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            foreach (var column in FeaturesGrid.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}
