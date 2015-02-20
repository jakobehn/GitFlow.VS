using System.Windows;
using System.Windows.Controls;
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
