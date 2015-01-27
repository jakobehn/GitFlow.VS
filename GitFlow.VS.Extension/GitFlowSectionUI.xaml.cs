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
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        public GitFlowSectionUI(GitFlowSection parent)
        {
            this.parent = parent;
            InitializeComponent();

            DataContext = this;
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
