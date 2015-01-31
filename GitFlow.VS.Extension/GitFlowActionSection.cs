using GitFlowVS.Extension.UI;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.topSection, GuidList.gitFlowPage, 100)]
    public class GitFlowActionSection : TeamExplorerBaseSection
    {
        public GitFlowActionSection()
        {
            Title = "Recommended actions";
            this.SectionContent = new GitFlowActionsUI();
        }   
    }
}