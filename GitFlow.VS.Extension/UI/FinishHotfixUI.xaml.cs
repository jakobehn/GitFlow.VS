using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for FinishHotfixUI.xaml
    /// </summary>
    public partial class FinishHotfixUI : UserControl
    {
       private readonly GitFlowSection parent;
        private readonly FinishHotfixModel model;
        private IGitRepositoryInfo ActiveRepo { get; set; }
        private IVsOutputWindowPane OutputWindow { get; set; }
        public FinishHotfixUI(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            model = new FinishHotfixModel();
            this.parent = parent;
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            InitializeComponent();
            DataContext = model;

            model.CurrentHotfix = CurrentFeature;
        }

        private void FinishHotfixCancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
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

        private async void FinishHotfixOK_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                progress.Visibility = Visibility.Visible;

                await Task.Run(() =>
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    gf.FinishHotfix(gf.CurrentBranchLeafName, model.TagMessage, model.DeleteBranch, model.ForceDeletion,
                        model.PushChanges);
                });
                progress.Visibility = Visibility.Hidden;
                parent.FinishAction();
            }
        }
    }
}
