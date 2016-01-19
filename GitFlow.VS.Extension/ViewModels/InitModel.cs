using System;
using System.Windows;
using System.Windows.Input;
using GitFlow.VS;

namespace GitFlowVS.Extension.ViewModels
{
    public class InitModel : ViewModelBase
    {
        private string master;
        private string develop;
        private string featurePrefix;
        private string releasePrefix;
        private string hotfixPrefix;
        private string versionTagPrefix;
        private Visibility initGridVisibility;

        public ICommand OnShowInitCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public InitModel(IGitFlowSection parent)
            :base(parent)
        {
            InitializeModel();

            OkCommand = new RelayCommand(p => OnInitialize(), p => true);
            CancelCommand = new RelayCommand(p=> OnCancel(), p => true);
            OnShowInitCommand = new RelayCommand(p => OnShowInit(), p => true);
        }

        private void InitializeModel()
        {
            Master = "master";
            Develop = "develop";
            FeaturePrefix = "feature/";
            ReleasePrefix = "release/";
            HotfixPrefix = "hotfix/";
            VersionTagPrefix = "";

            InitGridVisibility = Visibility.Hidden;
            ProgressVisibility = Visibility.Hidden; 
        }

        private void OnCancel()
        {
            InitializeModel();
        }

        public Visibility InitGridVisibility
        {
            get { return initGridVisibility; }
            set
            {
                if (value == initGridVisibility) return;
                initGridVisibility = value;
                OnPropertyChanged();
            }
        }

        private void OnShowInit()
        {
            InitGridVisibility = Visibility.Visible;
        }

        private void OnInitialize()
        {
            try
            {
                DateTime start = DateTime.Now;

                Logger.Event("Init");
                if (GitFlowPage.ActiveRepo != null)
                {
                    GitFlowPage.OutputWindow.Activate();
                    ProgressVisibility = Visibility.Visible;

                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindow);
                    var result = gf.Init(new GitFlowRepoSettings
                    {
                        DevelopBranch = Develop,
                        MasterBranch = Master,
                        FeatureBranch = FeaturePrefix,
                        ReleaseBranch = ReleasePrefix,
                        HotfixBranch = HotfixPrefix,
                        VersionTag = VersionTagPrefix
                    });
                    if (!result.Success)
                    {
                        Te.ShowErrorNotification(result.CommandOutput);
                    }

                    ProgressVisibility = Visibility.Hidden;
                    InitGridVisibility = Visibility.Hidden;
                    Te.Refresh();
                }
                Logger.Metric("Duration-Init", (DateTime.Now - start).Milliseconds);
            }
            catch (Exception e)
            {
                Te.ShowErrorNotification(e.ToString());
                Logger.Exception(e);
            }
		}

        public string Master
        {
            get { return master; }
            set
            {
                if (value == master) return;
                master = value;
                OnPropertyChanged();
            }
        }

        public string Develop
        {
            get { return develop; }
            set
            {
                if (value == develop) return;
                develop = value;
                OnPropertyChanged();
            }
        }

        public string FeaturePrefix
        {
            get { return featurePrefix; }
            set
            {
                if (value == featurePrefix) return;
                featurePrefix = value;
                OnPropertyChanged();
            }
        }

        public string ReleasePrefix
        {
            get { return releasePrefix; }
            set
            {
                if (value == releasePrefix) return;
                releasePrefix = value;
                OnPropertyChanged();
            }
        }

        public string HotfixPrefix
        {
            get { return hotfixPrefix; }
            set
            {
                if (value == hotfixPrefix) return;
                hotfixPrefix = value;
                OnPropertyChanged();
            }
        }

        public string VersionTagPrefix
        {
            get { return versionTagPrefix; }
            set
            {
                if (value == versionTagPrefix) return;
                versionTagPrefix = value;
                OnPropertyChanged();
            }
        }

        public void Update()
        {
            OnPropertyChanged("InitGridVisibility");
            OnPropertyChanged("OnShowInitCommand");
        }
    }
}