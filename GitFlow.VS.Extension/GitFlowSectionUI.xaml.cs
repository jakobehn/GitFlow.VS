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
        public GitFlowSectionUI()
        {
            InitializeComponent();
        }

        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
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
                    gf.Init(new GitFlowRepoSettings());
                }
            }
        }

        private void StartFeature_Click(object sender, RoutedEventArgs e)
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
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.ToString());
            }
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
                    gf.FinishFeature(FeatureName.Text);
                }
            }
        }
    }
}
