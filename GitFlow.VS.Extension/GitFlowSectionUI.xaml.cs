using System;
using System.Windows;
using System.Windows.Controls;
using GitFlow.VS;
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
        private readonly GitFlowViewModel model;

        public GitFlowSectionUI()
        {
            InitializeComponent();

            InitGrid.Visibility = Visibility.Collapsed;
            FeatureGrid.Visibility = Visibility.Collapsed;
        }

        public GitFlowSectionUI(GitFlowViewModel model)
        {
            this.model = model;
            InitializeComponent();

            DataContext = model;
        }

        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitGrid.Visibility = Visibility.Visible;
        }

        private void StartFeature_Click(object sender, RoutedEventArgs e)
        {
            FeatureGrid.Visibility = Visibility.Visible;
        }

        private void FinishFeature_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                using (new WaitCursor())
                {
                    var gf = new GitFlowWrapper(ActiveRepo.RepositoryPath);
                    gf.CommandOutputDataReceived += (o, args) =>
                    {
                        OutputWindow.OutputStringThreadSafe(args.Output);
                    };
                    gf.FinishFeature("TEST");
                }
            }
        }

        private void StartRelease_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FinishRelease_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StartHotfix_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FinishHotfix_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            InitGrid.Visibility = Visibility.Collapsed;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                using (new WaitCursor())
                {
                    var gf = new GitFlowWrapper(ActiveRepo.RepositoryPath);
                    gf.CommandOutputDataReceived += (o, args) =>
                    {
                        OutputWindow.OutputStringThreadSafe(args.Output);
                    };
                    gf.Init(new GitFlowRepoSettings()
                    {
                        DevelopBranch = DevelopBranch.Text,
                        MasterBranch = MasterBranch.Text,
                        FeatureBranch = FeatureBranchPrefix.Text,
                        ReleaseBranch = ReleaseBranchPrefix.Text,
                        HotfixBranch = HotfixBranchPrefix.Text,
                        VersionTag = VersionTagPrefix.Text
                    });
                    InitGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void OnCreateFeature(object sender, RoutedEventArgs e)
        {
                try
                {
                    if (ActiveRepo != null)
                    {
                        OutputWindow.Activate();
                        using (new WaitCursor())
                        {
                            var gf = new GitFlowWrapper(ActiveRepo.RepositoryPath);
                            gf.CommandOutputDataReceived += (o, args) =>
                            {
                                OutputWindow.OutputStringThreadSafe(args.Output);
                            };
                            gf.StartFeature(FeatureName.Text);
                        }
                        FeatureName.Clear();
                        FeatureGrid.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error: " + exception.ToString());
                }
        }

        private void OnCancelFeature(object sender, RoutedEventArgs e)
        {
            FeatureGrid.Visibility = Visibility.Collapsed;  

        }

    }
}
