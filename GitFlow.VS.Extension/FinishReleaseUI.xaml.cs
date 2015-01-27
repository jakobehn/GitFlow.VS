using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    public partial class FinishReleaseUI : UserControl
    {
       private readonly GitFlowSection parent;
        private readonly FinishReleaseModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }
        public FinishReleaseUI(GitFlowSection parent)
        {
            this.model = new FinishReleaseModel();
            this.parent = parent;
            InitializeComponent();

            this.DataContext = model;
        }

        private void FinishFeatureCancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void FinishReleaseOK_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                using (new WaitCursor())
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                    gf.FinishRelease(gf.CurrentBranch, model.TagMessage, model.DeleteBranch, model.ForceDeletion, model.PushChanges);
                }
                parent.FinishAction();
            }
        }
    }
}
