using System;
using GitFlowVS.Extension.UI;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.GitFlowInstallSection, GuidList.GitFlowPage, 100)]
    public class GitFlowInstallSection : TeamExplorerBaseSection, IGitFlowSection
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
                HandleException(e);
            }
        }

		private void HandleException(Exception ex)
		{
			Logger.Exception(ex);
			ShowNotification(ex.Message, NotificationType.Error);
		}

		public override void Refresh()
        {
            var service = GetService<ITeamExplorerPage>();
            service.Refresh();
        }

        public void ShowErrorNotification(string message)
        {
            ShowNotification(message, NotificationType.Error);
        }

        public void UpdateVisibleState()
        {
            IsVisible = !GitFlowPage.GitFlowIsInstalled;
        }
    }
}