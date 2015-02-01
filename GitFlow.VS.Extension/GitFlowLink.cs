using System;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerNavigationItem("59168500-14BC-4EE9-BB1F-7B2B970A4AF6",900)]
    public class GitFlowNavigationItem : TeamExplorerBaseNavigationItem
    {
        [ImportingConstructor]
        public GitFlowNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.IsVisible = true;

            this.Text = "GitFlow";
            this.Image = Resources.icon;
        }

        public override void Execute()
        {
            var service = this.GetService<ITeamExplorer>();
            if (service == null)
            {
                return;
            }
            service.NavigateToPage(new Guid(GuidList.gitFlowPage), null);
        }
    }
}
