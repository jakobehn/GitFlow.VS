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
        private StartFeatureModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        public StartFeatureUI(GitFlowSection parent)
        {
            this.model = new StartFeatureModel();
            this.parent = parent;
            InitializeComponent();

            this.DataContext = model;
        }

        private void OnCancelFeature(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void OnCreateFeature(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                OutputWindow.Activate();
                using (new WaitCursor())
                {
                    var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                    gf.StartFeature(model.FeatureName);
                }
                parent.FinishAction();
            }
        }
    }
}
