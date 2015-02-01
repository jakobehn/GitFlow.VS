using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using GitFlowVS.Extension.UI;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    //[TeamExplorerSection(GuidList.sampleTeamExplorerSection, TeamExplorerPageIds.Home, 100)]
    public class GitFlowSection : TeamExplorerBaseSection
    {
        private IServiceProvider serviceProvider;
        private IVsOutputWindowPane customPane;

        private IGitExt gitService;
        private IGitRepositoryInfo activeRepo;
        private GitFlowSectionUI gitFlowUi;

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {            
            base.Initialize(sender, e);

            Title = "GitFlow";

            serviceProvider = e.ServiceProvider;
            gitService = (IGitExt)e.ServiceProvider.GetService(typeof(IGitExt));
            gitService.PropertyChanged += GitServiceOnPropertyChanged;
            activeRepo = gitService.ActiveRepositories.FirstOrDefault();

            var outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            var customGuid = new Guid("B85225F6-B15E-4A8A-AF6E-2BE96A4FE672");
            outWindow.CreatePane(ref customGuid, "GitFlow.VS", 1, 1);            
            outWindow.GetPane(ref customGuid, out customPane);

            if (!GitFlowIsInstalled)
            {
                SectionContent = new InstallGitFlowUI(this);
            }
            else
            {
                gitFlowUi = new GitFlowSectionUI(this, activeRepo, customPane);
                SectionContent = gitFlowUi;
            }
        }

        public bool GitFlowIsInstalled
        {
            get
            {
                string gitFlowFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Git\\bin\\git-flow");
                if (!File.Exists(gitFlowFile))
                    return false;
                //Check if extension has been configured
                string binariesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Dependencies/binaries");
                return Directory.Exists(binariesPath);
;            }
        }

        private void GitServiceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (gitFlowUi != null)
            {
                gitFlowUi.UpdateModel(true);
            }
        }

        public void CancelAction()
        {
            var ui = new GitFlowSectionUI(this, activeRepo, customPane); 
            SectionContent = ui;
        }

        public void Init()
        {
            var ui = new InitUi();
            SectionContent = ui;
        }

        public void StartFeature()
        {
            //var ui = new StartFeatureUI(this, activeRepo, customPane);
            //SectionContent = ui;
        }

        public void FinishFeature()
        {
            var ui = new FinishFeatureUI(this,activeRepo,customPane);
            SectionContent = ui;
        }

        public void StartRelease()
        {
//            var ui = new StartReleaseUI(this, activeRepo, customPane);
  //          SectionContent = ui;
        }

        public void FinishRelease()
        {
            var ui = new FinishReleaseUI(this, activeRepo, customPane);
            SectionContent = ui;
        }

        public void StartHotfix()
        {
            //var ui = new StartHotfixUI(this, activeRepo, customPane);
            //SectionContent = ui;
        }

        public void FinishHotfix()
        {
            var ui = new FinishHotfixUI(this, activeRepo, customPane);
            SectionContent = ui;
        }

        public void FinishAction()
        {
            SectionContent = new GitFlowSectionUI(this, activeRepo, customPane); 
        }
    }
}
