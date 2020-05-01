using GitFlowVS.Extension.UI;
using GitFlowVS.Extension.ViewModels;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.GitFlowBugfixesSection, GuidList.GitFlowPage, 115)]
    public class GitFlowBugfixesSection : TeamExplorerBaseSection, IGitFlowSection
    {
        private readonly BugfixesViewModel model;

        public GitFlowBugfixesSection()
        {
            Title = "Current Bugfixes";
            IsVisible = false;
            model = new BugfixesViewModel(this);
            UpdateVisibleState();
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
                    SectionContent = new BugfixesUI(model);
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