using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using GitFlow.VS;
using GitFlowVS.Extension.Annotations;

namespace GitFlowVS.Extension.ViewModels
{
    public class FeaturesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<BranchItem> AllFeatures
        {
            get
            {
                var gf = new GitFlowWrapper(GitFlowPage.ActiveRepoPath);
                var list = gf.AllFeatureBranches.ToList();
                return list;
            }
        }

        public void Update()
        {
            OnPropertyChanged("AllFeatures");
        }

    }
}