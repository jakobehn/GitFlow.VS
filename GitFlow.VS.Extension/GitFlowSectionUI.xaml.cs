using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for GitFlowSectionUI.xaml
    /// </summary>
    public partial class GitFlowSectionUI : UserControl
    {
        private readonly GitFlowSection parent;
        private GitFlowViewModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        public GitFlowSectionUI(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            this.parent = parent;
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            InitializeComponent();

            this.model = new GitFlowViewModel();
            DataContext = model;

            model.CurrentState = CurrentState;
            UpdateModel();
        }

        public void UpdateModel()
        {
            if (ActiveRepo != null)
            {
                var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                model.InitVisible = gf.IsInitialized ? Visibility.Collapsed : Visibility.Visible;
                model.StartFeatureVisible = gf.IsInitialized && (gf.IsOnDevelopBranch || gf.IsOnMasterBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.FinishFeatureVisible = gf.IsInitialized && gf.IsOnFeatureBranch
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.StartReleaseVisible = gf.IsInitialized && (gf.IsOnDevelopBranch || gf.IsOnMasterBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.FinishReleaseVisible = gf.IsInitialized && gf.IsOnReleaseBranch
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.StartHotfixVisible = gf.IsInitialized && (gf.IsOnDevelopBranch || gf.IsOnMasterBranch)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                model.FinishHotfixVisible = gf.IsInitialized && gf.IsOnHotfixBranch ? Visibility.Visible : Visibility.Collapsed;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parent.Init();
        }

        private void StartFeature_Click(object sender, RoutedEventArgs e)
        {
            parent.StartFeature();
        }

        private void FinishFeature_Click(object sender, RoutedEventArgs e)
        {
            parent.FinishFeature();
        }

        private void StartRelease_Click(object sender, RoutedEventArgs e)
        {
            parent.StartRelease();
        }

        private void FinishRelease_Click(object sender, RoutedEventArgs e)
        {
            parent.FinishRelease();
        }

        private void StartHotfix_Click(object sender, RoutedEventArgs e)
        {
            parent.StartHotfix();
        }

        private void FinishHotfix_Click(object sender, RoutedEventArgs e)
        {
            parent.FinishHotfix();
        }
    }
}
