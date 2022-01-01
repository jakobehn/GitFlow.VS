using GitFlowVS.Extension.UI;
using GitFlowVS.Extension.ViewModels;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.GitFlowReleasesSection, GuidList.GitFlowPage, 125)]
    public class GitFlowReleasesSection : TeamExplorerBaseSection, IGitFlowSection
    {
        private readonly ReleasesViewModel model;

        public GitFlowReleasesSection()
        {
            Title = "Current Release";
            IsVisible = false;
            model = new ReleasesViewModel(this);
            UpdateVisibleState();
        }

        public void UpdateVisibleState()
        {
            if (!GitFlowPage.GitFlowIsInstalled || GitFlowPage.ActiveRepo == null)
            {
                IsVisible = false;
                return;
            }

            var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindow);
            if (gf.IsInitialized)
            {
                if (!IsVisible)
                {
                    SectionContent = new ReleasesUI(model);
                    IsVisible = true;
                }
                model.Update();
            }
            else
            {
                IsVisible = false;
            }
        }

    }
}