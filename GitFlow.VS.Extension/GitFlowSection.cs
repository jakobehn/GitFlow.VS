using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

namespace JakobEhn.GitFlow_VS_Extension
{
    [TeamExplorerSection(GuidList.sampleTeamExplorerSection, TeamExplorerPageIds.Home, 100)]
    public class GitFlowSection : TeamExplorerSectionBase
    {
        private IServiceProvider serviceProvider;

        private bool isBusy;

        private bool isExpanded = true;

        private bool isVisible = true;
        private IGitExt gitService;
        private IGitRepositoryInfo activeRepo;

        public GitFlowSection()
        {
        }
        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);
            this.serviceProvider = e.ServiceProvider;
            this.gitService = (IGitExt)e.ServiceProvider.GetService(typeof(IGitExt));
            gitService.PropertyChanged += GitServiceOnPropertyChanged;
            activeRepo = gitService.ActiveRepositories.FirstOrDefault();

             var ui = new GitFlowSectionUI();
            ui.ActiveRepo = activeRepo;
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

    public class WaitCursor : IDisposable
    {
        private System.Windows.Input.Cursor _previousCursor;

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}
