using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace GitFlowVS.Extension
{
    [TeamExplorerSection(GuidList.sampleTeamExplorerSection, TeamExplorerPageIds.Home, 100)]
    public class GitFlowSection : TeamExplorerSectionBase
    {
        private IServiceProvider serviceProvider;
        private IVsOutputWindowPane customPane;

        private bool isBusy;

        private bool isExpanded = true;

        private IGitExt gitService;
        private IGitRepositoryInfo activeRepo;

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);
            serviceProvider = e.ServiceProvider;
            gitService = (IGitExt)e.ServiceProvider.GetService(typeof(IGitExt));
            gitService.PropertyChanged += GitServiceOnPropertyChanged;
            activeRepo = gitService.ActiveRepositories.FirstOrDefault();

            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            Guid customGuid = new Guid("B85225F6-B15E-4A8A-AF6E-2BE96A4FE672");
            string customTitle = "GitFlow.VS";
            outWindow.CreatePane(ref customGuid, customTitle, 1, 1);            
            outWindow.GetPane(ref customGuid, out customPane);

            var ui = new GitFlowSectionUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        private void GitServiceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public void Cancel()
        {
        }

        public object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        public bool IsBusy
        {
            get { return isBusy; }
            private set
            {
                isBusy = value;
                FirePropertyChanged("IsBusy");
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                FirePropertyChanged("IsExpanded");
            }
        }

        

        public string Title
        {
            get { return "GitFlow"; }
        }

        public void Dispose()
        {
        }

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void FinishInit()
        {
            var ui = new GitFlowSectionUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void CancelAction()
        {
            var ui = new GitFlowSectionUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }


        public void Init()
        {
            var ui = new InitUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void StartFeature()
        {
            var ui = new StartFeatureUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void FinishFeature()
        {
            var ui = new FinishFeatureUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void StartRelease()
        {
            var ui = new StartReleaseUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void FinishRelease()
        {
            var ui = new FinishReleaseUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void StartHotfix()
        {
            var ui = new StartHotfixUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void FinishHotfix()
        {
            var ui = new FinishHotfixUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }

        public void FinishAction()
        {
            var ui = new GitFlowSectionUI(this);
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            SectionContent = ui;
        }
    }
}
