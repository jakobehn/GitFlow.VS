using System;
using System.Windows.Forms;
using GitFlowVS.Extension.UI;
using GitFlowVS.Extension.ViewModels;
using Microsoft.TeamFoundation.Controls;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.GitFlowInitSection, GuidList.GitFlowPage, 100)]
    public class GitFlowInitSection : TeamExplorerBaseSection, IGitFlowSection
    {
        private readonly InitModel model;

        public GitFlowInitSection()
        {
            try
            {
                model = new InitModel(this);
                Title = "Recommended actions";
                SectionContent = new InitUi(model);

                UpdateVisibleState();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void UpdateVisibleState()
        {
            if (!GitFlowPage.GitFlowIsInstalled || GitFlowPage.ActiveRepo == null)
            {
                IsVisible = false;
                return;
            }
            var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindow);
            IsVisible = !gf.IsInitialized;
            if (IsVisible)
            {
                model.Update();
            }
        }
    }
}