using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for GitFlowSectionUI.xaml
    /// </summary>
    public partial class GitFlowSectionUI : UserControl
    {
        private readonly GitFlowSection parent;
        private readonly GitFlowViewModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        public GitFlowSectionUI(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            this.parent = parent;
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            InitializeComponent();

            model = new GitFlowViewModel();
            DataContext = model;

            model.CurrentState = CurrentState;
            UpdateModel();
        }

        public void UpdateModel(bool reset = false)
        {
            if (ActiveRepo != null)
            {
                var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                model.InitVisible = gf.IsInitialized ? Visibility.Collapsed : Visibility.Visible;
                model.StartFeatureVisible = gf.IsInitialized && (model.ShowAll || gf.IsOnDevelopBranch || gf.IsOnMasterBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.FinishFeatureVisible = gf.IsInitialized && (model.ShowAll || gf.IsOnFeatureBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.StartReleaseVisible = gf.IsInitialized && (model.ShowAll || gf.IsOnDevelopBranch || gf.IsOnMasterBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.FinishReleaseVisible = gf.IsInitialized && (model.ShowAll || gf.IsOnReleaseBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.StartHotfixVisible = gf.IsInitialized && (model.ShowAll || gf.IsOnDevelopBranch || gf.IsOnMasterBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.FinishHotfixVisible = gf.IsInitialized && (model.ShowAll || gf.IsOnHotfixBranch) ? Visibility.Visible : Visibility.Collapsed;

                model.CurrentStateVisible = gf.IsInitialized &&
                                            (gf.IsOnFeatureBranch || gf.IsOnReleaseBranch || gf.IsOnHotfixBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                if (reset)
                {
                    model.ShowAll = false;
                }
                else
                {
                    if (!gf.IsInitialized)
                    {
                        model.ShowAll = false;
                    }
                }
            }
        }

        public string CurrentState
        {
            get
            {
                if (ActiveRepo != null)
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                    return gf.CurrentStatus;
                }
                return "";
            }
        }


    }
}
