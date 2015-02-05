using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GitFlow.VS;
using GitFlowVS.Extension.ViewModels;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    public partial class InitUi : UserControl
    {
        private readonly InitModel model;

        public InitUi(InitModel model)
        {
            this.model = model;
            InitializeComponent();
            DataContext = model;
        }

    }
}
