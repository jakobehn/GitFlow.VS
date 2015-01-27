using System;
using System.Windows;
using System.Windows.Controls;
using GitFlow.VS;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for InitUI.xaml
    /// </summary>
    public partial class InitUI : UserControl
    {
        private readonly GitFlowSection parent;
        private InitModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        public InitUI(GitFlowSection parent)
        {
            this.model = new InitModel();
            this.parent = parent;
            InitializeComponent();

            this.DataContext = model;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
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
                        DevelopBranch = model.Develop,
                        MasterBranch = model.Master,
                        FeatureBranch = model.FeaturePrefix,
                        ReleaseBranch = model.ReleasePrefix,
                        HotfixBranch = model.HotfixPrefix,
                        VersionTag = model.VersionTagPrefix
                    });
                }
            }
            parent.FinishInit();
        }
    }
}
