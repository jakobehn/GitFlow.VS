using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GitFlow.VS;
using GitFlowVS.Extension.Annotations;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using MessageBox = System.Windows.Forms.MessageBox;

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
        private ListItem selectedFeature;
        private bool hotfixTagMessageSelected;
        private bool releaseTagMessageSelected;
        private ListItem selectedHotfix;
        private ListItem selectedRelease;

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
            FeatureDeleteBranch = true;
            ReleaseDeleteBranch = true;
            ReleaseTagMessageSelected = true;
            HotfixDeleteBranch = true;
            HotfixTagMessageSelected = true;

            ShowStartFeature = Visibility.Collapsed;
            ShowStartRelease = Visibility.Collapsed;
            ShowStartHotfix = Visibility.Collapsed;
            
            ShowFinishFeature = Visibility.Collapsed;
            ShowFinishRelease = Visibility.Collapsed;
            ShowFinishHotfix = Visibility.Collapsed;

            ProgressVisibility = Visibility.Hidden;

            StartFeatureDropDownCommand = new DropDownLinkCommand(p => StartFeatureDropDown(), p => CanShowStartFeatureDropDown());
            StartFeatureCommand = new RelayCommand(p => StartFeature(), p => CanCreateFeature);
            CancelStartFeatureCommand = new RelayCommand(p => CancelStartFeature(), p => true);

            StartReleaseDropDownCommand = new DropDownLinkCommand(p => StartReleaseDropDown(), p => CanShowStartReleaseDropDown());
            StartReleaseCommand = new RelayCommand(p => StartRelease(), p => CanCreateRelease);
            CancelStartReleaseCommand = new RelayCommand(p => CancelStartRelease(), p => true);

            StartHotfixDropDownCommand = new DropDownLinkCommand(p => StartHotfixDropDown(), p => CanShowStartHotfixDropDown());
            StartHotfixCommand = new RelayCommand(p => StartHotfix(), p => CanCreateHotfix);
            CancelStartHotfixCommand = new RelayCommand(p => CancelStartHotfix(), p => true);

            FinishFeatureDropDownCommand = new DropDownLinkCommand(p => FinishFeatureDropDown(), p => CanShowFinishFeatureDropDown());
            FinishFeatureCommand = new RelayCommand(p => FinishFeature(), p => CanFinishFeature);
            CancelFinishFeatureCommand = new RelayCommand(p => CancelFinishFeature(), p => true);

            FinishReleaseDropDownCommand = new DropDownLinkCommand(p => FinishReleaseDropDown(), p => CanShowFinishReleaseDropDown());
            FinishReleaseCommand = new RelayCommand(p => FinishRelease(), p => CanFinishRelease);
            CancelFinishReleaseCommand = new RelayCommand(p => CancelFinishRelease(), p => true);

            FinishHotfixDropDownCommand = new DropDownLinkCommand(p => FinishHotfixDropDown(), p => CanShowFinishHotfixDropDown());
            FinishHotfixCommand = new RelayCommand(p => FinishHotfix(), p => CanFinishHotfix);
            CancelFinishHotfixCommand = new RelayCommand(p => CancelFinishHotfix(), p => true);
        }

        public bool CanFinishRelease
        {
            get { return SelectedRelease != null; }
        }

        public bool CanFinishHotfix
        {
            get { return SelectedHotfix != null; }
        }

        public bool CanFinishFeature
        {
            get { return SelectedFeature != null; }
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
            HideAll();
            ShowStartFeature = Visibility.Visible;
        }

        private void HideAll()
        {
            ShowStartFeature = Visibility.Collapsed;
            ShowStartRelease = Visibility.Collapsed;
            ShowStartHotfix = Visibility.Collapsed;
            ShowFinishFeature = Visibility.Collapsed;
            ShowFinishRelease = Visibility.Collapsed;
            ShowFinishHotfix = Visibility.Collapsed;
        }

        private void StartHotfixDropDown()
        {
            HideAll();
            ShowStartHotfix = Visibility.Visible;

        }
        private void StartReleaseDropDown()
        {
            HideAll();
            ShowStartRelease = Visibility.Visible;
        }

        private void FinishFeatureDropDown()
        {
            HideAll();
            ShowFinishFeature = Visibility.Visible;
        }

        private void FinishHotfixDropDown()
        {
            HideAll();
            ShowFinishHotfix = Visibility.Visible;
        }
        private void FinishReleaseDropDown()
        {
            HideAll();
            ShowFinishRelease = Visibility.Visible;
        }

        public bool CanCreateFeature
        {
            get { return !String.IsNullOrEmpty(FeatureName); }
        }

        public bool CanCreateRelease
        {
            get
            {
                return !String.IsNullOrEmpty(ReleaseName);
            }
        }

        public bool CanCreateHotfix
        {
            get { return !String.IsNullOrEmpty(HotfixName); }
        }

        public List<ListItem> AllFeatures
        {
            get
            {
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                return gf.AllFeatures.Select(x => new ListItem {Name = x}).ToList();
            }
        }

        public ListItem SelectedFeature
        {
            get { return selectedFeature; }
            set
            {
                if (Equals(value, selectedFeature)) return;
                selectedFeature = value;
                OnPropertyChanged();
            }
        }

        public List<ListItem> AllReleases
        {
            get
            {
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                return gf.AllReleases.Select(x => new ListItem { Name = x }).ToList();
            }
        }

        public ListItem SelectedRelease
        {
            get { return selectedRelease; }
            set
            {
                if (Equals(value, selectedRelease)) return;
                selectedRelease = value;
                OnPropertyChanged();
            }
        }

        public List<ListItem> AllHotfixes
        {
            get
            {
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                return gf.AllHotfixes.Select(x => new ListItem { Name = x }).ToList();
            }
        }


        public ListItem SelectedHotfix
        {
            get { return selectedHotfix; }
            set
            {
                if (Equals(value, selectedHotfix)) return;
                selectedHotfix = value;
                OnPropertyChanged();
            }
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
                SelectedFeature = null;
                FeatureName = String.Empty;
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
                SelectedRelease = null;
                ReleaseName = String.Empty;
            }
        }

        private async void StartHotfix()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;

                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.StartHotfix(HotfixName);

                ProgressVisibility = Visibility.Hidden;
                ShowStartHotfix = Visibility.Collapsed;
                HotfixName = String.Empty;
                SelectedHotfix = null;
            }
        }

        private void FinishFeature()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();

                ProgressVisibility = Visibility.Visible;

                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.FinishFeature(SelectedFeature.Name, FeatureRebaseOnDevelopmentBranch, FeatureDeleteBranch);

                ProgressVisibility = Visibility.Hidden;
                ShowFinishFeature = Visibility.Collapsed;
                OnPropertyChanged("AllFeatures");
                OnPropertyChanged("SelectedFeature");
            }
        }

        private void FinishRelease()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;

                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.FinishRelease(SelectedRelease.Name, ReleaseTagMessage, ReleaseDeleteBranch, ReleaseForceDeletion, ReleasePushChanges);
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
                gf.FinishHotfix(SelectedHotfix.Name, HotfixTagMessage, HotfixDeleteBranch, HotfixForceDeletion, HotfixPushChanges);
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
                OnPropertyChanged("CanCreateRelease");
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
                OnPropertyChanged("CanCreateHotfix");
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
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                if (gf.IsOnFeatureBranch)
                {
                    SelectedFeature = AllFeatures.First(f => f.Name == gf.CurrentBranchLeafName);
                }

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

        public bool HotfixTagMessageSelected
        {
            get { return hotfixTagMessageSelected; }
            set
            {
                if (value.Equals(hotfixTagMessageSelected)) return;
                hotfixTagMessageSelected = value;
                OnPropertyChanged();
            }
        }

        public bool ReleaseTagMessageSelected
        {
            get { return releaseTagMessageSelected; }
            set
            {
                if (value.Equals(releaseTagMessageSelected)) return;
                releaseTagMessageSelected = value;
                OnPropertyChanged();
            }
        }

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

        #region Visibility

        public Visibility StartFeatureVisible
        {
            get { return OnMainBranch(); }
        }

        public Visibility OtherStartFeatureVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnDevelopBranch || gf.IsOnMasterBranch ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility OtherStartReleaseVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnDevelopBranch || gf.IsOnMasterBranch ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility OtherStartHotfixVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnDevelopBranch || gf.IsOnMasterBranch ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private static Visibility OnMainBranch()
        {
            var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
            return gf.IsOnDevelopBranch || gf.IsOnMasterBranch ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility StartReleaseVisible
        {
            get { return OnMainBranch(); }
        }

        public Visibility StartHotfixVisible
        {
            get { return OnMainBranch(); }
        }

        public Visibility FinishHotfixVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnHotfixBranch ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility FinishFeatureVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnFeatureBranch ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility FinishReleaseVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnReleaseBranch ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        public Visibility OtherFinishHotfixVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnHotfixBranch ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility OtherFinishFeatureVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnFeatureBranch ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility OtherFinishReleaseVisible
        {
            get
            {
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                return gf.IsOnReleaseBranch ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        #endregion
    }
}
