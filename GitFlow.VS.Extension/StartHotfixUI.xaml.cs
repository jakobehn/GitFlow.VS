using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    /// <summary>
    /// Interaction logic for StartHotfixUI.xaml
    /// </summary>
    public partial class StartHotfixUI : UserControl
    {
        private readonly GitFlowSection parent;
        private StartHotfixModel model;
        public IGitRepositoryInfo ActiveRepo { get; set; }
        public IVsOutputWindowPane OutputWindow { get; set; }

        public StartHotfixUI(GitFlowSection parent)
        {
            this.parent = parent;
            InitializeComponent();

            model = new StartHotfixModel();
            DataContext = model;
        }

        private void HotfixCancel_Click(object sender, RoutedEventArgs e)
        {
            parent.CancelAction();
        }

        private void HotfixOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ActiveRepo != null)
                {
                    OutputWindow.Activate();
                    using (new WaitCursor())
                    {
                        var gf = new VsGitFlowWrapper(ActiveRepo.RepositoryPath, OutputWindow);
                        gf.StartHotfix(model.HotfixName);
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
