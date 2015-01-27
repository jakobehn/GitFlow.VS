using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for FinishFeatureUI.xaml
    /// </summary>
    public partial class FinishFeatureUI : UserControl
    {
        private readonly FinishFeatureModel model;
        private GitFlowSection parent;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }
        public FinishFeatureUI(GitFlowSection parent)
        {
            this.model = new FinishFeatureModel();
            this.parent = parent;
            InitializeComponent();

            this.DataContext = model;            
        }

        private void FinishFeature_Cancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void FinishFeature_OK_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                using (new WaitCursor())
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                    gf.FinishFeature(gf.CurrentBranchLeafName, model.RebaseOnDevelopment, model.DeleteBranch);
                }

                FinishFeatureGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
