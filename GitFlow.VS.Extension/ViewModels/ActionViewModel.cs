using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GitFlowVS.Extension.Annotations;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace GitFlowVS.Extension.ViewModels
{
    public class ActionViewModel : INotifyPropertyChanged
    {
        private Visibility showStartFeature;
        private Visibility showStartRelease;
        private Visibility showStartHotfix;

        private Visibility showFinishFeature;
        private Visibility showFinishRelease;
        private Visibility showFinishHotfix;

        private string featureName;
        private string releaseName;
        private string hotfixName;

        private Visibility progressVisibility;
        private bool featureRebaseOnDevelopmentBranch;
        private bool featureDeleteBranch;
        private bool releaseDeleteBranch;
        private string releaseTagMessage;
        private bool releaseForceDeletion;
        private bool releasePushChanges;
        private bool hotfixDeleteBranch;
        private bool hotfixPushChanges;
        private bool hotfixForceDeletion;
        private string hotfixTagMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand StartFeatureDropDownCommand { get; private set; }
        public ICommand StartFeatureCommand { get; private set; }
        public ICommand CancelStartFeatureCommand { get; private set; }


        public ICommand StartReleaseDropDownCommand { get; private set; }
        public ICommand StartReleaseCommand { get; private set; }
        public ICommand CancelStartReleaseCommand { get; private set; }

        public ICommand StartHotfixDropDownCommand { get; private set; }
        public ICommand StartHotfixCommand { get; private set; }
        public ICommand CancelStartHotfixCommand { get; private set; }




        public ICommand FinishFeatureDropDownCommand { get; private set; }
        public ICommand FinishFeatureCommand { get; private set; }
        public ICommand CancelFinishFeatureCommand { get; private set; }


        public ICommand FinishReleaseDropDownCommand { get; private set; }
        public ICommand FinishReleaseCommand { get; private set; }
        public ICommand CancelFinishReleaseCommand { get; private set; }

        public ICommand FinishHotfixDropDownCommand { get; private set; }
        public ICommand FinishHotfixCommand { get; private set; }
        public ICommand CancelFinishHotfixCommand { get; private set; }


        public ActionViewModel()
        {
            ShowStartFeature = Visibility.Collapsed;
            ShowStartRelease = Visibility.Collapsed;
            ShowStartHotfix = Visibility.Collapsed;
            
            ShowFinishFeature = Visibility.Collapsed;
            ShowFinishRelease = Visibility.Collapsed;
            ShowFinishHotfix = Visibility.Collapsed;


            ProgressVisibility = Visibility.Hidden;

            StartFeatureDropDownCommand = new DropDownLinkCommand(p => StartFeatureDropDown(), p => CanShowStartFeatureDropDown());
            StartFeatureCommand = new CommandHandler(StartFeature, true);
            CancelStartFeatureCommand = new CommandHandler(CancelStartFeature, true);

            StartReleaseDropDownCommand = new DropDownLinkCommand(p => StartReleaseDropDown(), p => CanShowStartReleaseDropDown());
            StartReleaseCommand = new CommandHandler(StartRelease, true);
            CancelStartReleaseCommand = new CommandHandler(CancelStartRelease, true);

            StartHotfixDropDownCommand = new DropDownLinkCommand(p => StartHotfixDropDown(), p => CanShowStartHotfixDropDown());
            StartHotfixCommand = new CommandHandler(StartHotfix, true);
            CancelStartHotfixCommand = new CommandHandler(CancelStartHotfix, true);


            FinishFeatureDropDownCommand = new DropDownLinkCommand(p => FinishFeatureDropDown(), p => CanShowFinishFeatureDropDown());
            FinishFeatureCommand = new CommandHandler(FinishFeature, true);
            CancelFinishFeatureCommand = new CommandHandler(CancelFinishFeature, true);

            FinishReleaseDropDownCommand = new DropDownLinkCommand(p => FinishReleaseDropDown(), p => CanShowFinishReleaseDropDown());
            FinishReleaseCommand = new CommandHandler(FinishRelease, true);
            CancelFinishReleaseCommand = new CommandHandler(CancelFinishRelease, true);

            FinishHotfixDropDownCommand = new DropDownLinkCommand(p => FinishHotfixDropDown(), p => CanShowFinishHotfixDropDown());
            FinishHotfixCommand = new CommandHandler(FinishHotfix, true);
            CancelFinishHotfixCommand = new CommandHandler(CancelFinishHotfix, true);
        }

        private void CancelStartFeature()
        {
            ShowStartFeature = Visibility.Collapsed;
        }
        private void CancelStartHotfix()
        {
            ShowStartHotfix = Visibility.Collapsed;
        }

        private void CancelStartRelease()
        {
            ShowStartRelease = Visibility.Collapsed;
        }

        private void CancelFinishFeature()
        {
            ShowFinishFeature = Visibility.Collapsed;
        }
        private void CancelFinishHotfix()
        {
            ShowFinishHotfix = Visibility.Collapsed;
        }

        private void CancelFinishRelease()
        {
            ShowFinishRelease = Visibility.Collapsed;
        }

        private bool CanShowStartFeatureDropDown()
        {
            return true;
        }
        private bool CanShowStartHotfixDropDown()
        {
            return true;
        }
        private bool CanShowStartReleaseDropDown()
        {
            return true;
        }

        private bool CanShowFinishFeatureDropDown()
        {
            return true;
        }
        private bool CanShowFinishHotfixDropDown()
        {
            return true;
        }
        private bool CanShowFinishReleaseDropDown()
        {
            return true;
        }

        private void StartFeatureDropDown()
        {
            ShowStartFeature = Visibility.Visible;
            ShowStartRelease = Visibility.Collapsed;
            ShowStartHotfix = Visibility.Collapsed;
        }
        private void StartHotfixDropDown()
        {
            ShowStartHotfix = Visibility.Visible;
            ShowStartRelease = Visibility.Collapsed;
            ShowStartFeature = Visibility.Collapsed;

        }
        private void StartReleaseDropDown()
        {
            ShowStartRelease = Visibility.Visible;
            ShowStartFeature = Visibility.Collapsed;
            ShowStartHotfix = Visibility.Collapsed;
        }

        private void FinishFeatureDropDown()
        {
            ShowFinishFeature = Visibility.Visible;
        }
        private void FinishHotfixDropDown()
        {
            ShowFinishHotfix = Visibility.Visible;
        }
        private void FinishReleaseDropDown()
        {
            ShowFinishRelease = Visibility.Visible;
        }

        public bool CanCreateFeature
        {
            get { return !String.IsNullOrEmpty(FeatureName); }
        }

        public bool CanCreateRelease
        {
            get { return !String.IsNullOrEmpty(ReleaseName); }
        }

        public bool CanCreateHotfix
        {
            get { return !String.IsNullOrEmpty(HotfixName); }
        }

        private void StartFeature()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.StartFeature(FeatureName);
                ProgressVisibility = Visibility.Hidden;
                ShowStartFeature =Visibility.Collapsed;
            }
        }

        private void StartRelease()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.StartRelease(ReleaseName);
                ProgressVisibility = Visibility.Hidden;
                ShowStartRelease = Visibility.Collapsed;
            }
        }

        private void StartHotfix()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.StartHotfix(HotfixName);
                ProgressVisibility = Visibility.Hidden;
                ShowStartHotfix = Visibility.Collapsed;
            }
        }

        private void FinishFeature()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();

                ProgressVisibility = Visibility.Visible;

                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.FinishFeature(gf.CurrentBranchLeafName, FeatureRebaseOnDevelopmentBranch, FeatureDeleteBranch);

                ProgressVisibility = Visibility.Hidden;
                ShowFinishFeature = Visibility.Collapsed;
            }
        }

        private void FinishRelease()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;

                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.FinishRelease(gf.CurrentBranchLeafName, ReleaseTagMessage, ReleaseDeleteBranch, ReleaseForceDeletion, ReleasePushChanges);
                ProgressVisibility = Visibility.Hidden;
                ShowFinishRelease = Visibility.Collapsed;
            }
            
        }

        private void FinishHotfix()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;

                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.FinishHotfix(gf.CurrentBranchLeafName, HotfixTagMessage, HotfixDeleteBranch, HotfixForceDeletion, HotfixPushChanges);
                ProgressVisibility = Visibility.Hidden;
                ShowFinishHotfix = Visibility.Collapsed;
            }
            
        }

        public string ReleaseName
        {
            get { return releaseName; }
            set
            {
                if (value == releaseName) return;
                releaseName = value;
                OnPropertyChanged();
            }
        }

        public string HotfixName
        {
            get { return hotfixName; }
            set
            {
                if (value == hotfixName) return;
                hotfixName = value;
                OnPropertyChanged();
            }
        }

        public string FeatureName
        {
            get { return featureName; }
            set
            {
                if (value == featureName) return;
                featureName = value;
                OnPropertyChanged();
                OnPropertyChanged("CanCreateFeature");
            }
        }

        public Visibility ProgressVisibility
        {
            get { return progressVisibility; }
            set
            {
                if (value == progressVisibility) return;
                progressVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowStartFeature
        {
            get { return showStartFeature; }
            set
            {
                if (value == showStartFeature) return;
                showStartFeature = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowStartHotfix
        {
            get { return showStartHotfix; }
            set
            {
                if (value == showStartHotfix) return;
                showStartHotfix = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowStartRelease
        {
            get { return showStartRelease; }
            set
            {
                if (value == showStartRelease) return;
                showStartRelease = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowFinishFeature
        {
            get { return showFinishFeature; }
            set
            {
                if (value == showFinishFeature) return;
                showFinishFeature = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowFinishHotfix
        {
            get { return showFinishHotfix; }
            set
            {
                if (value == showFinishHotfix) return;
                showFinishHotfix = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowFinishRelease
        {
            get { return showFinishRelease; }
            set
            {
                if (value == showFinishRelease) return;
                showFinishRelease = value;
                OnPropertyChanged();
            }
        }

        #region Feature

        public bool FeatureRebaseOnDevelopmentBranch
        {
            get { return featureRebaseOnDevelopmentBranch; }
            set
            {
                if (value.Equals(featureRebaseOnDevelopmentBranch)) return;
                featureRebaseOnDevelopmentBranch = value;
                OnPropertyChanged();
            }
        }

        public bool FeatureDeleteBranch
        {
            get { return featureDeleteBranch; }
            set
            {
                if (value.Equals(featureDeleteBranch)) return;
                featureDeleteBranch = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Release


        public string ReleaseTagMessage
        {
            get { return releaseTagMessage; }
            set
            {
                if (value == releaseTagMessage) return;
                releaseTagMessage = value;
                OnPropertyChanged();
            }
        }

        public bool ReleaseDeleteBranch
        {
            get { return releaseDeleteBranch; }
            set
            {
                if (value.Equals(releaseDeleteBranch)) return;
                releaseDeleteBranch = value;
                OnPropertyChanged();
            }
        }

        public bool ReleaseTagMessageEnabled
        {
            get { return String.IsNullOrEmpty(ReleaseTagMessage); }
        }

        public bool ReleasePushChanges
        {
            get { return releasePushChanges; }
            set
            {
                if (value.Equals(releasePushChanges)) return;
                releasePushChanges = value;
                OnPropertyChanged();
            }
        }

        public bool ReleaseForceDeletion
        {
            get { return releaseForceDeletion; }
            set
            {
                if (value.Equals(releaseForceDeletion)) return;
                releaseForceDeletion = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Hotfix

        public string HotfixTagMessage
        {
            get { return hotfixTagMessage; }
            set
            {
                if (value == hotfixTagMessage) return;
                hotfixTagMessage = value;
                OnPropertyChanged();
            }
        }

        public bool HotfixForceDeletion
        {
            get { return hotfixForceDeletion; }
            set
            {
                if (value.Equals(hotfixForceDeletion)) return;
                hotfixForceDeletion = value;
                OnPropertyChanged();
            }
        }

        public bool HotfixPushChanges
        {
            get { return hotfixPushChanges; }
            set
            {
                if (value.Equals(hotfixPushChanges)) return;
                hotfixPushChanges = value;
                OnPropertyChanged();
            }
        }

        public bool HotfixDeleteBranch
        {
            get { return hotfixDeleteBranch; }
            set
            {
                if (value.Equals(hotfixDeleteBranch)) return;
                hotfixDeleteBranch = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
