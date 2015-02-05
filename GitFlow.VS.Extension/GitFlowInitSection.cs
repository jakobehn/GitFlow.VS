using System;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.GitFlowInitSection, GuidList.GitFlowPage, 100)]
    public class GitFlowInitSection : TeamExplorerBaseSection
    {
        public GitFlowInitSection()
        {
            try
            {
                Title = "Recommended actions";
                SectionContent = new InitUi(this);

                UpdateVisibleState();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public override void Refresh()
        {
            UpdateVisibleState();
            var service = GetService<ITeamExplorerPage>();
            service.Refresh();
        }

        public void UpdateVisibleState()
        {
            if (!GitFlowPage.GitFlowIsInstalled)
            {
                IsVisible = false;
                return;
            }
            var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindow);
            IsVisible = !gf.IsInitialized;
        }
    }
}