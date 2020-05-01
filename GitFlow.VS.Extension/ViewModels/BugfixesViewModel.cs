using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GitFlow.VS;
using System;

namespace GitFlowVS.Extension.ViewModels
{
    public class BugfixesViewModel : ViewModelBase
    {
        public ICommand PublishBugfixBranchCommand { get; private set; }
        public ICommand TrackBugfixBranchCommand { get; private set; }
        public ICommand CheckoutBugfixBranchCommand { get; private set; }

        public BugfixesViewModel(IGitFlowSection te)
            : base(te)
        {
            PublishBugfixBranchCommand = new RelayCommand(p => PublishBugfixBranch(), p => CanPublishBugfixBranch);
            TrackBugfixBranchCommand = new RelayCommand(p => TrackBugfixBranch(), p => CanTrackBugfixBranch);
            CheckoutBugfixBranchCommand = new RelayCommand(p => CheckoutBugfixBranch(), p => CanCheckoutBugfixBranch);

            HideProgressBar();
        }

        public bool CanPublishBugfixBranch
        {
            get
            {
                return SelectedBugfix != null && !SelectedBugfix.IsRemote && !SelectedBugfix.IsTracking;
            }
        }

        public bool CanTrackBugfixBranch
        {
            get
            {
                return SelectedBugfix != null && SelectedBugfix.IsRemote && !SelectedBugfix.IsTracking;
            }
        }

        public bool CanCheckoutBugfixBranch
        {
            get
            {
                return SelectedBugfix != null && !SelectedBugfix.IsCurrentBranch && !SelectedBugfix.IsRemote;
            }
        }

        public void PublishBugfixBranch()
        {
            try
            {
                Logger.Event("PublishBugfixBranch");
                GitFlowPage.ActiveOutputWindow();
                ShowProgressBar();
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                var result = gf.PublishBugfix(SelectedBugfix.Name);
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

        public void TrackBugfixBranch()
        {
            try
            {
                Logger.Event("TrackBugfixBranch");
                GitFlowPage.ActiveOutputWindow();
                ShowProgressBar();
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                var result = gf.TrackBugfix(SelectedBugfix.Name);
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

        public void CheckoutBugfixBranch()
        {
            try
            {
                Logger.Event("CheckoutBugfixBranch");
                GitFlowPage.ActiveOutputWindow();
                ShowProgressBar();
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                var result = gf.CheckoutBugfix(SelectedBugfix.Name);
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


        public List<BranchItem> AllBugfixes
        {
            get
            {
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                var list = gf.AllBugfixBranches.ToList();
                return list;
            }
        }

        public BranchItem SelectedBugfix { get; set; }

        public void Update()
        {
            OnPropertyChanged("AllBugfixes");
            OnPropertyChanged("NoItemsMessageVisibility");
        }

        public Visibility NoItemsMessageVisibility
        {
            get { return AllBugfixes.Any() ? Visibility.Collapsed : Visibility.Visible; }

        }

        private void ShowErrorMessage(string message)
        {
            Te.ShowErrorNotification(message);
        }
    }
}