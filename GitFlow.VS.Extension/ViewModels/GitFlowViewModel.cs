using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public class GitFlowViewModel :INotifyPropertyChanged
    {
        private Visibility initVisible;
        private string currentState;
        private Visibility startFeatureVisible;
        private Visibility finishFeatureVisible;
        private Visibility startReleaseVisible;
        private Visibility finishReleaseVisible;
        private Visibility startHotfixVisible;
        private Visibility finishHotfixVisible;
        private Visibility currentStateVisible;
        private bool showAll;
        public event PropertyChangedEventHandler PropertyChanged;

        public GitFlowViewModel()
        {
            ShowAll = false;
        }
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Visibility InitVisible
        {
            get { return initVisible; }
            set
            {
                if (value == initVisible) return;
                initVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowAllVisible
        {
            get { return ShowAll || InitVisible == Visibility.Visible  ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility CurrentStateVisible
        {
            get { return currentStateVisible; }
            set
            {
                if (value == currentStateVisible) return;
                currentStateVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility StartFeatureVisible
        {
            get { return startFeatureVisible; }
            set
            {
                if (value == startFeatureVisible) return;
                startFeatureVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility FinishFeatureVisible
        {
            get { return finishFeatureVisible; }
            set
            {
                if (value == finishFeatureVisible) return;
                finishFeatureVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility StartReleaseVisible
        {
            get { return startReleaseVisible; }
            set
            {
                if (value == startReleaseVisible) return;
                startReleaseVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility FinishReleaseVisible
        {
            get { return finishReleaseVisible; }
            set
            {
                if (value == finishReleaseVisible) return;
                finishReleaseVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility StartHotfixVisible
        {
            get { return startHotfixVisible; }
            set
            {
                if (value == startHotfixVisible) return;
                startHotfixVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility FinishHotfixVisible
        {
            get { return finishHotfixVisible; }
            set
            {
                if (value == finishHotfixVisible) return;
                finishHotfixVisible = value;
                OnPropertyChanged();
            }
        }

        public string CurrentState
        {
            get { return currentState; }
            set
            {
                if (value == currentState) return;
                currentState = value;
                OnPropertyChanged();
            }
        }

        public bool ShowAll
        {
            get { return showAll; }
            set
            {
                if (value.Equals(showAll)) return;
                showAll = value;
                OnPropertyChanged();
                OnPropertyChanged("ShowAllVisible");
            }
        }
    }
}