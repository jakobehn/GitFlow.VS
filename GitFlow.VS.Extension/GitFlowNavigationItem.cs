using System;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerNavigationItem("59168500-14BC-4EE9-BB1F-7B2B970A4AF6",1500)]
    public class GitFlowNavigationItem : TeamExplorerBaseNavigationItem
    {
        [ImportingConstructor]
        public GitFlowNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            IsVisible = true;
            Text = "GitFlow";
            Image = Resources.LinkIcon;
        }

        public override void Execute()
        {
            var service = GetService<ITeamExplorer>();
            if (service == null)
            {
                return;
            }
            service.NavigateToPage(new Guid(GuidList.GitFlowPage), null);
        }
    }
}
