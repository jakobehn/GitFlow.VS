using GitFlowVS.Extension.UI;
using GitFlowVS.Extension.ViewModels;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.GitFlowFeaturesSection, GuidList.GitFlowPage, 120)]
    public class GitFlowFeaturesSection : TeamExplorerBaseSection, IGitFlowSection
    {
        private readonly FeaturesViewModel model;

        public GitFlowFeaturesSection()
        {
            Title = "Current Features";
            IsVisible = false;
            model = new FeaturesViewModel();
            UpdateVisibleState();
        }

        public override void Refresh()
        {
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
            if (gf.IsInitialized)
            {
                if (!IsVisible)
                {
                    SectionContent = new FeaturesUI(model);
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
            ShowNotification(message, NotificationType.Error);
        }

    }
}