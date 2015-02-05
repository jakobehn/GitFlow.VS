using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GitFlow.VS;
using GitFlowVS.Extension.Annotations;
using TeamExplorer.Common;

namespace GitFlowVS.Extension.ViewModels
{
    public class InitModel : INotifyPropertyChanged
    {
        private readonly TeamExplorerBaseSection parent;
        private string master;
        private string develop;
        private string featurePrefix;
        private string releasePrefix;
        private string hotfixPrefix;
        private string versionTagPrefix;
        private Visibility progressVisibility;
        private Visibility initGridVisibility;

        public ICommand OnShowInitCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public InitModel(TeamExplorerBaseSection parent)
        {
            this.parent = parent;
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

        private void OnShowInit()
        {
            InitGridVisibility = Visibility.Visible;
        }

        private void OnInitialize()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.OutputWindow.Activate();
                ProgressVisibility = Visibility.Visible;

                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepo.RepositoryPath, GitFlowPage.OutputWindow);
                gf.Init(new GitFlowRepoSettings
                {
                    DevelopBranch = Develop,
                    MasterBranch = Master,
                    FeatureBranch = FeaturePrefix,
                    ReleaseBranch = ReleasePrefix,
                    HotfixBranch = HotfixPrefix,
                    VersionTag = VersionTagPrefix
                });

                ProgressVisibility = Visibility.Hidden;
                InitGridVisibility = Visibility.Hidden;
                parent.Refresh();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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

    }
}