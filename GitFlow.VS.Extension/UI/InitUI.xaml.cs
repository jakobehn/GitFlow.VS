using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GitFlow.VS;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    public partial class InitUi : UserControl
    {
        private readonly GitFlowSection parent;
        private readonly InitModel model;
        private IGitRepositoryInfo ActiveRepo { get; set; }
        private IVsOutputWindowPane OutputWindow { get; set; }

        public InitUi(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            model = new InitModel();
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            this.parent = parent;
            InitializeComponent();
            DataContext = model;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                loadingPanel.IsLoading = true;
                loadingPanel.Message = "Initializing GitFlow";
                //loadingPanel.Visibility = Visibility.Visible;
                
                await Task.Run(() =>
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    gf.Init(new GitFlowRepoSettings
                    {
                        DevelopBranch = model.Develop,
                        MasterBranch = model.Master,
                        FeatureBranch = model.FeaturePrefix,
                        ReleaseBranch = model.ReleasePrefix,
                        HotfixBranch = model.HotfixPrefix,
                        VersionTag = model.VersionTagPrefix
                    });
                });

                loadingPanel.IsLoading = false;

            }
            parent.FinishAction();
        }
    }
}
