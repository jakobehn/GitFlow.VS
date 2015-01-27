using System.ComponentModel;
using System.Runtime.CompilerServices;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public class InitModel : INotifyPropertyChanged
    {
        private string master;
        private string develop;
        private string featurePrefix;
        private string releasePrefix;
        private string hotfixPrefix;
        private string versionTagPrefix;

        public InitModel()
        {
            Master = "master";
            Develop = "develop";
            FeaturePrefix = "feature/";
            ReleasePrefix = "release/";
            HotfixPrefix = "hotfix/";
            VersionTagPrefix = "";
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