using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public enum CurrentStateEnum
    {
        Master = 0,
        Develop = 1,
        Feature = 2,
        Release = 3,
        Hotfix = 4
    }

    public class GitFlowViewModel : INotifyPropertyChanged
    {
        private CurrentStateEnum currentState;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public CurrentStateEnum CurrentState
        {
            get { return currentState; }
            set
            {
                if (value == currentState) return;
                currentState = value;
                OnPropertyChanged();
            }
        }

    }
}
