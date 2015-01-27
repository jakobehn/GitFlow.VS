using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.sampleTeamExplorerSection, TeamExplorerPageIds.Home, 100)]
    public class GitFlowSection : TeamExplorerBaseSection
    {
        private IServiceProvider serviceProvider;
        private IVsOutputWindowPane customPane;

        private IGitExt gitService;
        private IGitRepositoryInfo activeRepo;
        private GitFlowSectionUI ui;

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);

            this.Title = "GitFlow";

            serviceProvider = e.ServiceProvider;
            gitService = (IGitExt)e.ServiceProvider.GetService(typeof(IGitExt));
            gitService.PropertyChanged += GitServiceOnPropertyChanged;
            activeRepo = gitService.ActiveRepositories.FirstOrDefault();

            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            Guid customGuid = new Guid("B85225F6-B15E-4A8A-AF6E-2BE96A4FE672");
            string customTitle = "GitFlow.VS";
            outWindow.CreatePane(ref customGuid, customTitle, 1, 1);            
            outWindow.GetPane(ref customGuid, out customPane);

            this.ui = new GitFlowSectionUI(this, activeRepo, customPane); 
            SectionContent = ui;

        }
        

        private void GitServiceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (ui != null)
            {
                ui.UpdateModel();
            }
        }

        public void CancelAction()
        {
            var ui = new GitFlowSectionUI(this, activeRepo, customPane); 
            SectionContent = ui;
        }

        public void Init()
        {
            var ui = new InitUI(this) {ActiveRepo = activeRepo, OutputWindow = customPane};
            SectionContent = ui;
        }

        public void StartFeature()
        {
            var ui = new StartFeatureUI(this) {ActiveRepo = activeRepo, OutputWindow = customPane};
            SectionContent = ui;
        }

        public void FinishFeature()
        {
            var ui = new FinishFeatureUI(this) {ActiveRepo = activeRepo, OutputWindow = customPane};
            SectionContent = ui;
        }

        public void StartRelease()
        {
            var ui = new StartReleaseUI(this) {ActiveRepo = activeRepo, OutputWindow = customPane};
            SectionContent = ui;
        }

        public void FinishRelease()
        {
            var ui = new FinishReleaseUI(this) {ActiveRepo = activeRepo, OutputWindow = customPane};
            SectionContent = ui;
        }

        public void StartHotfix()
        {
            var ui = new StartHotfixUI(this) {ActiveRepo = activeRepo, OutputWindow = customPane};
            SectionContent = ui;
        }

        public void FinishHotfix()
        {
            var ui = new FinishHotfixUI(this) {ActiveRepo = activeRepo, OutputWindow = customPane};
            SectionContent = ui;
        }

        public void FinishAction()
        {
            SectionContent = new GitFlowSectionUI(this, activeRepo, customPane); 
        }
    }
}
