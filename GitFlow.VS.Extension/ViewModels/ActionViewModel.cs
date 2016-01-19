using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GitFlow.VS;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;

namespace GitFlowVS.Extension.ViewModels
{
    public class ActionViewModel : ViewModelBase
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


        public ActionViewModel(GitFlowActionSection te)
            : base(te)
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

            HideProgressBar();

            StartFeatureDropDownCommand = new DropDownLinkCommand(p => StartFeatureDropDown(), p => CanShowStartFeatureDropDown());
            StartFeatureCommand = new RelayCommand(p => StartFeature(), p => CanCreateFeature);
            CancelStartFeatureCommand = new RelayCommand(p => CancelStartFeature(), p => CanCancelFinishCommand());

            StartReleaseDropDownCommand = new DropDownLinkCommand(p => StartReleaseDropDown(), p => CanShowStartReleaseDropDown());
            StartReleaseCommand = new RelayCommand(p => StartRelease(), p => CanCreateRelease);
            CancelStartReleaseCommand = new RelayCommand(p => CancelStartRelease(), p => CanCancelFinishCommand());

            StartHotfixDropDownCommand = new DropDownLinkCommand(p => StartHotfixDropDown(), p => CanShowStartHotfixDropDown());
            StartHotfixCommand = new RelayCommand(p => StartHotfix(), p => CanCreateHotfix);
            CancelStartHotfixCommand = new RelayCommand(p => CancelStartHotfix(), p => CanCancelFinishCommand());

            FinishFeatureDropDownCommand = new DropDownLinkCommand(p => FinishFeatureDropDown(), p => CanShowFinishFeatureDropDown());
            FinishFeatureCommand = new RelayCommand(p => FinishFeature(), p => CanFinishFeature);
            CancelFinishFeatureCommand = new RelayCommand(p => CancelFinishFeature(), p => CanCancelFinishCommand());

            FinishReleaseDropDownCommand = new DropDownLinkCommand(p => FinishReleaseDropDown(), p => CanShowFinishReleaseDropDown());
            FinishReleaseCommand = new RelayCommand(p => FinishRelease(), p => CanFinishRelease);
            CancelFinishReleaseCommand = new RelayCommand(p => CancelFinishRelease(), p => CanCancelFinishCommand());

            FinishHotfixDropDownCommand = new DropDownLinkCommand(p => FinishHotfixDropDown(), p => CanShowFinishHotfixDropDown());
            FinishHotfixCommand = new RelayCommand(p => FinishHotfix(), p => CanFinishHotfix);
            CancelFinishHotfixCommand = new RelayCommand(p => CancelFinishHotfix(), p => CanCancelFinishCommand());
        }

        public bool CanFinishRelease
        {
            get { return SelectedRelease != null && CanCancelFinishCommand(); }
        }

        public bool CanFinishHotfix
        {
            get { return SelectedHotfix != null && CanCancelFinishCommand(); }
        }

        public bool CanFinishFeature
        {
            get { return SelectedFeature != null && CanCancelFinishCommand(); }
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

        private bool CanCancelFinishCommand()
        {
            return ProgressVisibility != Visibility.Visible;
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

        private void UpdateMenus()
        {
            OnPropertyChanged("StartFeatureVisible");
            OnPropertyChanged("StartReleaseVisible");
            OnPropertyChanged("StartHotfixVisible");
            OnPropertyChanged("FinishFeatureVisible");
            OnPropertyChanged("FinishReleaseVisible");
            OnPropertyChanged("FinishHotfixVisible");

            OnPropertyChanged("OtherStartFeatureVisible");
            OnPropertyChanged("OtherStartReleaseVisible");
            OnPropertyChanged("OtherStartHotfixVisible");
            OnPropertyChanged("OtherFinishFeatureVisible");
            OnPropertyChanged("OtherFinishReleaseVisible");
            OnPropertyChanged("OtherFinishHotfixVisible");
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
            get { return !String.IsNullOrEmpty(FeatureName) && CanCancelFinishCommand(); }
        }

        public bool CanCreateRelease
        {
            get
            {
                return !String.IsNullOrEmpty(ReleaseName) && CanCancelFinishCommand();
            }
        }

        public bool CanCreateHotfix
        {
            get { return !String.IsNullOrEmpty(HotfixName) && CanCancelFinishCommand(); }
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
            try
            { 
                if (String.IsNullOrEmpty(FeatureName))
                    return;

			    Logger.Event("StartFeature");
	            DateTime start = DateTime.Now;

                if (GitFlowPage.ActiveRepo != null)
                {
                    GitFlowPage.ActiveOutputWindow();
                    ShowProgressBar();
                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                    var result = gf.StartFeature(FeatureName);
                    if (!result.Success)
                    {
                        ShowErrorMessage(result);
                    }

                    HideProgressBar();
                    FeatureName = String.Empty;
                    UpdateMenus();
                    HideAll();
                    OnPropertyChanged("AllFeatures");
                }
			    Logger.Metric("Duration-StartFeature", (DateTime.Now-start).Milliseconds);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.ToString());
                Logger.Exception(ex);
            }

        }

        private void StartRelease()
        {
            try
            { 
                if (String.IsNullOrEmpty(ReleaseName))
                    return;

			    Logger.Event("StartRelease");
	            DateTime start = DateTime.Now;

			    if (GitFlowPage.ActiveRepo != null)
                {
                    GitFlowPage.ActiveOutputWindow();
                    ShowProgressBar();
                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                    var result = gf.StartRelease(ReleaseName);
                    if (!result.Success)
                    {
                        ShowErrorMessage(result);
                    }
                    HideProgressBar();
                    ShowStartRelease = Visibility.Collapsed;
                    ReleaseName = String.Empty;
                    UpdateMenus();
                    HideAll();
                    OnPropertyChanged("AllReleases");
                }
			    Logger.Metric("Duration-StartRelease", (DateTime.Now - start).Milliseconds);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.ToString());
                Logger.Exception(ex);
            }

        }

        private void ShowErrorMessage(GitFlowCommandResult result)
        {
            Te.ShowErrorNotification(result.CommandOutput);
        }
        private void ShowErrorMessage(string message)
        {
            Te.ShowErrorNotification(message);
        }

        private void StartHotfix()
        {
            try
            { 
                if (String.IsNullOrEmpty(HotfixName))
                    return;

			    Logger.Event("StartHotfix");
	            DateTime start = DateTime.Now;

			    if (GitFlowPage.ActiveRepo != null)
                {
                    GitFlowPage.ActiveOutputWindow();
                    ShowProgressBar();

                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                    var result = gf.StartHotfix(HotfixName);
                    if (!result.Success)
                    {
                        ShowErrorMessage(result);
                    }

                    HideProgressBar();
                    ShowStartHotfix = Visibility.Collapsed;
                    HotfixName = String.Empty;
                    UpdateMenus();
                    HideAll();
                    OnPropertyChanged("AllHotfixes");
                }
			    Logger.Metric("Duration-StartHotfix", (DateTime.Now - start).Milliseconds);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.ToString());
                Logger.Exception(ex);
            }

        }

        private void FinishFeature()
        {
            try
            {
                DateTime start = DateTime.Now;

                var properties = new Dictionary<string, string>
                {
                    {"RebaseOnDevelopmentBranch", FeatureRebaseOnDevelopmentBranch.ToString()},
                    {"DeleteBranch", FeatureDeleteBranch.ToString()}
                };
                Logger.Event("FinishFeature", properties);

                if (GitFlowPage.ActiveRepo != null)
                {
                    GitFlowPage.ActiveOutputWindow();

                    ShowProgressBar();

                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                    var result = gf.FinishFeature(SelectedFeature.Name, FeatureRebaseOnDevelopmentBranch, FeatureDeleteBranch);
                    if (!result.Success)
                    {
                        ShowErrorMessage(result);
                    }

                    HideProgressBar();
                    ShowFinishFeature = Visibility.Collapsed;
                    UpdateMenus();
                    HideAll();
                    OnPropertyChanged("AllFeatures");
                    Te.Refresh();
                }

                Logger.Metric("Duration-FinishFeature", (DateTime.Now - start).Milliseconds);
            }
            catch( Exception ex)
            {
                ShowErrorMessage(ex.ToString());
                Logger.Exception(ex);
            }
		}

        private void FinishRelease()
        {
            try
            { 
			    DateTime start = DateTime.Now;
			    var properties = new Dictionary<string, string>
			    {
				    {"TaggedRelease", (!String.IsNullOrEmpty(ReleaseTagMessage)).ToString()},
				    {"DeleteBranch", ReleaseDeleteBranch.ToString()},
				    {"ForceDeletion", ReleaseForceDeletion.ToString()},
				    {"PushChanges", ReleasePushChanges.ToString()}
			    };
			    Logger.Event("FinishRelease", properties);

			    if (GitFlowPage.ActiveRepo != null)
                {
                    GitFlowPage.ActiveOutputWindow();
                    ShowProgressBar();

                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                    var result = gf.FinishRelease(SelectedRelease.Name, ReleaseTagMessage, ReleaseDeleteBranch, ReleaseForceDeletion, ReleasePushChanges);
                    if (!result.Success)
                    {
                        ShowErrorMessage(result);
                    }

                    HideAll();
                    HideProgressBar();
                    ShowFinishRelease = Visibility.Collapsed;
                    OnPropertyChanged("AllReleases");
                    UpdateMenus();
                }
			    Logger.Metric("Duration-FinishRelease", (DateTime.Now - start).Milliseconds);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.ToString());
                Logger.Exception(ex);
            }
        }

        private void FinishHotfix()
        {
            try
            { 
			    DateTime start = DateTime.Now;
			    var properties = new Dictionary<string, string>
			    {
				    {"TaggedRelease", (!String.IsNullOrEmpty(HotfixTagMessage)).ToString()},
				    {"DeleteBranch", HotfixDeleteBranch.ToString()},
				    {"ForceDeletion", HotfixForceDeletion.ToString()},
				    {"PushChanges", HotfixPushChanges.ToString()}
			    };
			    Logger.Event("FinishHotfix", properties);

			    if (GitFlowPage.ActiveRepo != null)
                {
                    GitFlowPage.ActiveOutputWindow();
                    ShowProgressBar();

                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                    var result = gf.FinishHotfix(SelectedHotfix.Name, HotfixTagMessage, HotfixDeleteBranch, HotfixForceDeletion, HotfixPushChanges);
                    if (!result.Success)
                    {
                        ShowErrorMessage(result);
                    }

                    HideAll();
                    HideProgressBar();
                    ShowFinishHotfix = Visibility.Collapsed;
                    OnPropertyChanged("AllHotfixes");
                    UpdateMenus();
                }
			    Logger.Metric("Duration-FinishHotfix", (DateTime.Now - start).Milliseconds);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.ToString());
                Logger.Exception(ex);
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
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                if (gf.IsOnHotfixBranch)
                {
                    SelectedHotfix = AllHotfixes.First(f => f.Name == gf.CurrentBranchLeafName);
                }
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

                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                if (gf.IsOnReleaseBranch)
                {
                    SelectedRelease = AllReleases.First(f => f.Name == gf.CurrentBranchLeafName);
                }
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

        //TODO: Refactor this code and introduce BooleanToVisibility converter...
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
            get
            {
                return OnMainBranch();
            }
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

        public void Update()
        {
            UpdateMenus();
        }
    }
}
