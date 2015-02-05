using System.Windows.Controls;
using GitFlowVS.Extension.ViewModels;

namespace GitFlowVS.Extension.UI
{
    public partial class GitFlowActionsUI : UserControl
    {
        private ActionViewModel model;
        public GitFlowActionsUI(ActionViewModel model)
        {
            this.model = model;
            InitializeComponent();
            DataContext = model;
        }
    }
}
