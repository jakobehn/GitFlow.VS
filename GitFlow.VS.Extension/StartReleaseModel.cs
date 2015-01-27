using System.ComponentModel;
using System.Runtime.CompilerServices;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public class StartReleaseModel : INotifyPropertyChanged
    {
        private string releaseName;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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
    }
}