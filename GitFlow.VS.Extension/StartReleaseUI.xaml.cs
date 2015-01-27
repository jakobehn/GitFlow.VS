using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for StartReleaseUI.xaml
    /// </summary>
    public partial class StartReleaseUI : UserControl
    {
        private readonly GitFlowSection parent;
        private StartReleaseModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }
        public StartReleaseUI(GitFlowSection parent)
        {
            this.model = new StartReleaseModel();
            this.parent = parent;
            InitializeComponent();

            this.DataContext = model;
        }

        private void OnCancelRelease(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void OnCreateRelease(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ActiveRepo != null)
                {
                    OutputWindow.Activate();
                    using (new WaitCursor())
                    {
                        var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                        gf.StartRelease(model.ReleaseName);
                    }
                    parent.FinishAction();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
    }
}
