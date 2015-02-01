using System;
using System.Threading.Tasks;
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
            model.CurrentRelease = CurrentFeature;
        }

        public string CurrentFeature
        {
            get
            {
                if (ActiveRepo != null)
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                    return gf.CurrentBranchLeafName;
                }
                return "";
            }
        }

        private void FinishFeatureCancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private async void FinishReleaseOK_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                progress.Visibility = Visibility.Visible;

                await Task.Run(() =>
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                    gf.FinishRelease(gf.CurrentBranchLeafName, model.TagMessage, model.DeleteBranch, model.ForceDeletion,
                        model.PushChanges);
                });
                progress.Visibility = Visibility.Hidden;
                parent.FinishAction();
            }
        }
    }
}
