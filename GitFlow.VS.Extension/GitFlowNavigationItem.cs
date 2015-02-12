using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerNavigationItem("59168500-14BC-4EE9-BB1F-7B2B970A4AF6",1500)]
    public class GitFlowNavigationItem : TeamExplorerBaseNavigationItem
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITeamExplorer teamExplorer;
        private readonly IGitExt gitService;

        [ImportingConstructor]
        public GitFlowNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            try
            {
                this.serviceProvider = serviceProvider;
                UpdateVisible();
                Text = "GitFlow";
                Image = Resources.LinkIcon;
                IsVisible = true;
                teamExplorer = GetService<ITeamExplorer>();
                gitService = (IGitExt)serviceProvider.GetService(typeof(IGitExt));
                teamExplorer.PropertyChanged += TeamExplorerOnPropertyChanged;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        protected override void ContextChanged(object sender, ContextChangedEventArgs e)
        {
            UpdateVisible();
            base.ContextChanged(sender, e);
        }

        private void TeamExplorerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateVisible();
        }

        private void UpdateVisible()
        {
            IsVisible = false;
            if (gitService != null && gitService.ActiveRepositories.Any())
            {
                IsVisible = true;
            }
        }

        public override void Execute()
        {
            teamExplorer.NavigateToPage(new Guid(GuidList.GitFlowPage), null);
        }
    }
}
