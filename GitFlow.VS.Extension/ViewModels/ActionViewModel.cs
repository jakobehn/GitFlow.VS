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

        private void StartFeature()
        {
            if (GitFlowPage.ActiveRepo != null)
            {
                GitFlowPage.ActiveOutputWindow();
                ProgressVisibility = Visibility.Visible;
                var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                gf.StartFeature(FeatureName);
                ProgressVisibility = Visibility.Hidden;
                ShowStartFeature =Visibility.Hidden;
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
                ShowStartRelease = Visibility.Hidden;
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
                ShowStartHotfix = Visibility.Hidden;
            }
        }

        private void FinishFeature()
        {
        }

        private void FinishRelease()
        {
            
        }

        private void FinishHotfix()
        {
            
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

    }
}
