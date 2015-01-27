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
        private readonly GitFlowSection parent;

        public GitFlowSectionUI(GitFlowSection parent)
        {
            this.parent = parent;
            InitializeComponent();
            StartFeatureGrid.Visibility = Visibility.Collapsed;
            FinishFeatureGrid.Visibility = Visibility.Collapsed;
        }

        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         //   InitGrid.Visibility = Visibility.Visible;
            parent.Init();
        }

        private void StartFeature_Click(object sender, RoutedEventArgs e)
        {
            StartFeatureGrid.Visibility = Visibility.Visible;
        }

        private void FinishFeature_Click(object sender, RoutedEventArgs e)
        {
            FinishFeatureGrid.Visibility = Visibility.Visible;
        }

        private GitFlowWrapper GetWrapper()
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                var gf = new GitFlowWrapper(ActiveRepo.RepositoryPath);
                gf.CommandOutputDataReceived += (o, args) =>
                {
                    OutputWindow.OutputStringThreadSafe(args.Output);
                };
                return gf;
            }
            return null;
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

        private void OnCreateFeature(object sender, RoutedEventArgs e)
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
            }
        }

        private void OnCancelFeature(object sender, RoutedEventArgs e)
        {
            StartFeatureGrid.Visibility = Visibility.Collapsed;
        }

        private void OnFinishFeature(object sender, RoutedEventArgs e)
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
                        gf.FinishFeature(gf.CurrentBranch, RebaseOnDevelopment.IsChecked.Value, DeleteBranch.IsChecked.Value);
                    }
                    FeatureName.Clear();
                    FinishFeatureGrid.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.ToString());
            }
        }

        private void OnCancelFinishFeature(object sender, RoutedEventArgs e)
        {
            FinishFeatureGrid.Visibility = Visibility.Collapsed;
        }
    }
}
