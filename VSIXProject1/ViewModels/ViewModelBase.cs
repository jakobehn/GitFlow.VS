using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected readonly IGitFlowSection Te;
        private Visibility progressVisibility;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Visibility ProgressVisibility
        {
            get { return progressVisibility; }
            protected set
            {
                if (value == progressVisibility) return;
                progressVisibility = value;
                OnPropertyChanged();
            }
        }

        protected void HideProgressBar()
        {
            ProgressVisibility = Visibility.Collapsed;
        }

        protected void ShowProgressBar()
        {
            ProgressVisibility = Visibility.Visible;
        }

        public ViewModelBase(IGitFlowSection te)
        {
            Te = te;
        }
    }
}