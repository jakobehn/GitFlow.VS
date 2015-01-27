using System.ComponentModel;
using System.Runtime.CompilerServices;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public class StartFeatureModel : INotifyPropertyChanged
    {
        private string featureName;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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
    }
}