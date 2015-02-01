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
    /// Interaction logic for GitFlowActionsUI.xaml
    /// </summary>
    public partial class GitFlowActionsUI : UserControl
    {
        private ActionViewModel model;

        public GitFlowActionsUI()
        {
            InitializeComponent();

            this.model = new ActionViewModel();
            DataContext = model;

        }
    }
}
