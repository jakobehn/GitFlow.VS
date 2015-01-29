using System.ComponentModel;
using System.Runtime.CompilerServices;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public class FinishFeatureModel : INotifyPropertyChanged
    {
        private bool rebaseOnDevelopment;
        private bool deleteBranch;
        private string currentFeature;
        public event PropertyChangedEventHandler PropertyChanged;

        public FinishFeatureModel()
        {
            RebaseOnDevelopment = false;
            DeleteBranch = true;
        }
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string CurrentFeature
        {
            get { return currentFeature; }
            set
            {
                if (value == currentFeature) return;
                currentFeature = value;
                OnPropertyChanged();
            }
        }

        public bool RebaseOnDevelopment
        {
            get { return rebaseOnDevelopment; }
            set
            {
                if (value.Equals(rebaseOnDevelopment)) return;
                rebaseOnDevelopment = value;
                OnPropertyChanged();
            }
        }

        public bool DeleteBranch
        {
            get { return deleteBranch; }
            set
            {
                if (value.Equals(deleteBranch)) return;
                deleteBranch = value;
                OnPropertyChanged();
            }
        }
    }
}