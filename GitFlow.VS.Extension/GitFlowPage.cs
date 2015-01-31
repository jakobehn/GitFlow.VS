using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerPage(GuidList.gitFlowPage, Undockable = true)]
    public class GitFlowPage : TeamExplorerBasePage
    {
        public GitFlowPage()
        {
            Title = "GitFlow";
        }
    }
}
