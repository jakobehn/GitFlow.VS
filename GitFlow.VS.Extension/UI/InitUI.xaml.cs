using System.Windows.Controls;
using GitFlowVS.Extension.ViewModels;

namespace GitFlowVS.Extension.UI
{
    public partial class InitUi : UserControl
    {
        private readonly InitModel model;

        public InitUi(InitModel model)
        {
			Logger.PageView("Init");

			this.model = model;
            InitializeComponent();
            DataContext = model;
        }

    }
}
