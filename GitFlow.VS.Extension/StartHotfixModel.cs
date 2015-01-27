using System.ComponentModel;
using System.Runtime.CompilerServices;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public class StartHotfixModel : INotifyPropertyChanged
    {
        private string hotfixName;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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
    }
}