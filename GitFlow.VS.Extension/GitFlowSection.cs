using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace JakobEhn.GitFlow_VS_Extension
{
    [TeamExplorerSection(GuidList.sampleTeamExplorerSection, TeamExplorerPageIds.Home, 10)]
    public class GitFlowSection : TeamExplorerSectionBase
    {
        private IServiceProvider serviceProvider;

        private bool isBusy;

        private bool isExpanded = true;

        private bool isVisible = true;

        public GitFlowSection()
        {
            this.SectionContent = new GitFlowUI();
        }
        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);
            this.serviceProvider = e.ServiceProvider;            
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
