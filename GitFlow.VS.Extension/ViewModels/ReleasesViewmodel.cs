using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GitFlow.VS;
using System;

namespace GitFlowVS.Extension.ViewModels
{

    public class ReleasesViewModel : ViewModelBase
    {
        public ICommand PublishReleaseBranchCommand { get; private set; }

        public ReleasesViewModel(IGitFlowSection te)
            : base(te)
        {
			PublishReleaseBranchCommand = new RelayCommand(p => PublishReleaseBranch(), p => CanPublishReleaseBranch);
            HideProgressBar();
        }

        public bool CanPublishReleaseBranch
        {
            get
            {
                return SelectedRelease != null && !SelectedRelease.IsRemote && !SelectedRelease.IsTracking;
            }
        }

        public void PublishReleaseBranch()
        {
            try
            { 
			    Logger.Event("PublishReleaseBranch");
			    GitFlowPage.ActiveOutputWindow();
                ShowProgressBar();
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                var result = gf.PublishRelease(SelectedRelease.Name);
                if (!result.Success)
                {
                    Te.ShowErrorNotification(result.CommandOutput);
                }

                HideProgressBar();
                Update();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.ToString());
                Logger.Exception(ex);
            }

        }

       

        public List<BranchItem> AllReleases
        {
            get
            {
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                var list = gf.AllReleaseBranches.ToList();
                return list;
            }
        }

        public BranchItem SelectedRelease { get; set; }

        public void Update()
        {
            OnPropertyChanged("AllReleases");
            OnPropertyChanged("NoItemsMessageVisibility");
        }

        public Visibility NoItemsMessageVisibility
        {
            get { return AllReleases.Any() ? Visibility.Collapsed : Visibility.Visible; }

        }

        private void ShowErrorMessage(string message)
        {
            Te.ShowErrorNotification(message);
        }

    }
}