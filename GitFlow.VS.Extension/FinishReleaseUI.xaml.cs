using System;
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
        private IGitRepositoryInfo ActiveRepo { get; set; }
        private IVsOutputWindowPane OutputWindow { get; set; }
        public FinishReleaseUI(GitFlowSection parent,IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            model = new FinishReleaseModel();
            this.parent = parent;
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            InitializeComponent();

            DataContext = model;
            model.CurrentFeature = CurrentFeature;
        }

        public string CurrentFeature
        {
            get
            {
                if (ActiveRepo != null)
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    return gf.CurrentBranchLeafName;
                }
                return "";
            }
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
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    gf.FinishRelease(gf.CurrentBranchLeafName, model.TagMessage, model.DeleteBranch, model.ForceDeletion, model.PushChanges);
                }
                parent.FinishAction();
            }
        }
    }
}
