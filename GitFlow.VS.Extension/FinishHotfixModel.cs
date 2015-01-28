using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension
{
    public class FinishHotfixModel : INotifyPropertyChanged
    {
        private string tagMessage;
        private bool deleteBranch;
        private bool forceDeletion;
        private bool pushChanges;
        private string currentFeature;
        public event PropertyChangedEventHandler PropertyChanged;

        public FinishHotfixModel()
        {
            DeleteBranch = true;
            ForceDeletion = false;
            PushChanges = false;

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

        public string TagMessage
        {
            get { return tagMessage; }
            set
            {
                if (value == tagMessage) return;
                tagMessage = value;
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

        public bool ForceDeletion
        {
            get { return forceDeletion; }
            set
            {
                if (value.Equals(forceDeletion)) return;
                forceDeletion = value;
                OnPropertyChanged();
            }
        }

        public bool PushChanges
        {
            get { return pushChanges; }
            set
            {
                if (value.Equals(pushChanges)) return;
                pushChanges = value;
                OnPropertyChanged();
            }
        }

        public bool TagMessageEnabled
        {
            get { return String.IsNullOrEmpty(TagMessage); }
        }
    }
}