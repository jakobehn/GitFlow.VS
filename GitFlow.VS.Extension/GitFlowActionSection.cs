using System.Runtime.InteropServices;
using GitFlowVS.Extension.UI;
using GitFlowVS.Extension.ViewModels;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.topSection, GuidList.gitFlowPage, 110)]
    public class GitFlowActionSection : TeamExplorerBaseSection
    {
        private ActionViewModel model;

        public GitFlowActionSection()
        {
            Title = "Recommended actions";
            IsVisible = false;
            model = new ActionViewModel(this);
            UpdateVisibleState();
        }

        public override void Refresh()
        {
            UpdateVisibleState();
        }

        public void UpdateVisibleState()
        {
            var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindow);
            if (gf.IsInitialized)
            {
                if (!IsVisible)
                {
                    SectionContent = new GitFlowActionsUI(model);
                    IsVisible = true;
                }
                model.Update();
            }
            else
            {
                IsVisible = false;
            }
        }

        public void ShowErrorNotification(string message)
        {
            this.ShowNotification(message, NotificationType.Error);
        }

    }
}