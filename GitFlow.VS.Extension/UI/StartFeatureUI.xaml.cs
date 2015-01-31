using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for StartFeatureUI.xaml
    /// </summary>
    public partial class StartFeatureUI : UserControl
    {
        private readonly GitFlowSection parent;
        private readonly StartFeatureModel model;
        private IGitRepositoryInfo ActiveRepo { get; set; }
        private IVsOutputWindowPane OutputWindow { get; set; }

        public StartFeatureUI()
        {
            
        }
        public StartFeatureUI(GitFlowSection parent, IGitRepositoryInfo activeRepo, IVsOutputWindowPane outputWindow)
        {
            model = new StartFeatureModel();
            ActiveRepo = activeRepo;
            OutputWindow = outputWindow;
            this.parent = parent;
            InitializeComponent();

            DataContext = model;
        }

        private void OnCancelFeature(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private async void OnCreateFeature(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                progress.Visibility = Visibility.Visible;
                await Task.Run(() =>
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow, parent);
                    gf.StartFeature(model.FeatureName);
                });
                progress.Visibility = Visibility.Hidden;
                parent.FinishAction();
            }
        }
    }
}
