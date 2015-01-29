using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for FinishFeatureUI.xaml
    /// </summary>
    public partial class FinishFeatureUI : UserControl
    {
        private readonly FinishFeatureModel model;
        private readonly GitFlowSection parent;
        private IGitRepositoryInfo ActiveRepo { get; set; }
        private IVsOutputWindowPane OutputWindow { get; set; }
        public FinishFeatureUI(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            model = new FinishFeatureModel();
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


        private void FinishFeature_Cancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private async void FinishFeature_OK_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();

                progress.Visibility = Visibility.Visible;
                
                await System.Threading.Tasks.Task.Run(() =>
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    gf.FinishFeature(gf.CurrentBranchLeafName, model.RebaseOnDevelopment, model.DeleteBranch);
                });

                progress.Visibility = Visibility.Hidden;
                parent.FinishAction();
            }
        }
    }
}
