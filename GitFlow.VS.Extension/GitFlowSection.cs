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

        private bool isVisible = true;
        private IGitExt gitService;
        private IGitRepositoryInfo activeRepo;

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);
            this.serviceProvider = e.ServiceProvider;
            this.gitService = (IGitExt)e.ServiceProvider.GetService(typeof(IGitExt));
            gitService.PropertyChanged += GitServiceOnPropertyChanged;
            activeRepo = gitService.ActiveRepositories.FirstOrDefault();

            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            // Use e.g. Tools -> Create GUID to make a stable, but unique GUID for your pane.
            // Also, in a real project, this should probably be a static constant, and not a local variable
            Guid customGuid = new Guid("B85225F6-B15E-4A8A-AF6E-2BE96A4FE672");
            string customTitle = "GitFlow.VS";
            outWindow.CreatePane(ref customGuid, customTitle, 1, 1);
            
            outWindow.GetPane(ref customGuid, out customPane);

            var ui = new GitFlowSectionUI();
            ui.ActiveRepo = activeRepo;
            ui.OutputWindow = customPane;
            this.SectionContent = ui;

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
            get { return this.isBusy; }
            private set
            {
                this.isBusy = value;
                this.FirePropertyChanged("IsBusy");
            }
        }

        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                this.isExpanded = value;
                this.FirePropertyChanged("IsExpanded");
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
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
