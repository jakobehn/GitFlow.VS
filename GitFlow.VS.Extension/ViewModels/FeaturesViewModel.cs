using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GitFlow.VS;
using System;

namespace GitFlowVS.Extension.ViewModels
{

    public class FeaturesViewModel : ViewModelBase
    {
        public ICommand PublishFeatureBranchCommand { get; private set; }
        public ICommand TrackFeatureBranchCommand { get; private set; }
        public ICommand CheckoutFeatureBranchCommand { get; private set; }

        public FeaturesViewModel(IGitFlowSection te)
            : base(te)
        {
            PublishFeatureBranchCommand = new RelayCommand(p => PublishFeatureBranch(), p => CanPublishFeatureBranch);
            TrackFeatureBranchCommand = new RelayCommand(p => TrackFeatureBranch(), p => CanTrackFeatureBranch);
            CheckoutFeatureBranchCommand = new RelayCommand(p => CheckoutFeatureBranch(), p => CanCheckoutFeatureBranch);

            HideProgressBar();
        }

        public bool CanPublishFeatureBranch
        {
            get
            {
                return SelectedFeature != null && !SelectedFeature.IsRemote && !SelectedFeature.IsTracking;
            }
        }

        public bool CanTrackFeatureBranch
        {
            get
            {
                return SelectedFeature != null && SelectedFeature.IsRemote && !SelectedFeature.IsTracking;
            }
        }

        public bool CanCheckoutFeatureBranch
        {
            get
            {
                return SelectedFeature != null && !SelectedFeature.IsCurrentBranch && !SelectedFeature.IsRemote;
            }
        }

        public void PublishFeatureBranch()
        {
            try
            { 
			    Logger.Event("PublishFeatureBranch");
			    GitFlowPage.ActiveOutputWindow();
                ShowProgressBar();
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                var result = gf.PublishFeature(SelectedFeature.Name);
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

        public void TrackFeatureBranch()
        {
            try
            { 
			    Logger.Event("TrackFeatureBranch");
			    GitFlowPage.ActiveOutputWindow();
                ShowProgressBar();
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                var result = gf.TrackFeature(SelectedFeature.Name);
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

        public void CheckoutFeatureBranch()
        {
            try
            { 
			    Logger.Event("CheckoutFeatureBranch");
			    GitFlowPage.ActiveOutputWindow();
                ShowProgressBar();
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                var result = gf.CheckoutFeature(SelectedFeature.Name);
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


        public List<BranchItem> AllFeatures
        {
            get
            {
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                var list = gf.AllFeatureBranches.ToList();
                return list;
            }
        }

        public BranchItem SelectedFeature { get; set; }

        public void Update()
        {
            OnPropertyChanged("AllFeatures");
            OnPropertyChanged("NoItemsMessageVisibility");
        }

        public Visibility NoItemsMessageVisibility
        {
            get { return AllFeatures.Any() ? Visibility.Collapsed : Visibility.Visible; }

        }

        private void ShowErrorMessage(string message)
        {
            Te.ShowErrorNotification(message);
        }

    }
}