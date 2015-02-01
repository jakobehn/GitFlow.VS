using System;
using System.Windows.Forms;
using GitFlowVS.Extension.UI;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.topSection, GuidList.gitFlowPage, 110)]
    public class GitFlowActionSection : TeamExplorerBaseSection
    {
        public GitFlowActionSection()
        {
            Title = "Recommended actions";
            SectionContent = new GitFlowActionsUI();
            UpdateVisibleState();
        }

        public override void Refresh()
        {
            UpdateVisibleState();
        }

        private void UpdateVisibleState()
        {
            var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindowPane);
            IsVisible = gf.IsInitialized;
        }

    }

    [TeamExplorerSection(GuidList.initSection, GuidList.gitFlowPage, 100)]
    public class GitFlowInitSection : TeamExplorerBaseSection
    {
        public GitFlowInitSection()
        {
            try
            {
                Title = "Recommended actions";
                SectionContent = new InitUi();

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
        }

        private void UpdateVisibleState()
        {
            var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindowPane);
            IsVisible = !gf.IsInitialized;
        }
    }
}