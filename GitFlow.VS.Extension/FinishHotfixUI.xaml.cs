using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for FinishHotfixUI.xaml
    /// </summary>
    public partial class FinishHotfixUI : UserControl
    {
       private readonly GitFlowSection parent;
        private readonly FinishHotfixModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }
        public FinishHotfixUI(GitFlowSection parent)
        {
            this.model = new FinishHotfixModel();
            this.parent = parent;
            InitializeComponent();
            this.DataContext = model;
        }

        private void FinishHotfixCancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void FinishHotfixOK_Click(object sender, RoutedEventArgs e)
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
