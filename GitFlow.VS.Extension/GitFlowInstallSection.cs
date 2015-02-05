using System;
using GitFlowVS.Extension.UI;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.GitFlowInstallSection, GuidList.GitFlowPage, 100)]
    public class GitFlowInstallSection : TeamExplorerBaseSection
    {
        public GitFlowInstallSection()
        {
            try
            {
                Title = "Install GitFlow";
                SectionContent = new InstallGitFlowUI(this);

                UpdateVisibleState();

            }
            catch (Exception e)
            {
                ShowNotification(e.ToString(), NotificationType.Error);
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
            IsVisible = !GitFlowPage.GitFlowIsInstalled;
        }
    }
}