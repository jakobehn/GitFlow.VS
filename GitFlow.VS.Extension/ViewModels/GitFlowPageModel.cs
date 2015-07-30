using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GitFlowVS.Extension.Annotations;
using Microsoft.TeamFoundation.Controls;

namespace GitFlowVS.Extension.ViewModels
{
    public class GitFlowPageModel : INotifyPropertyChanged
    {
        public ICommand SelectBranchCommand { get; private set; }

        public string CurrentBranch 
        {
            get
            {
                if (!String.IsNullOrEmpty(GitFlowPage.ActiveRepoPath))
                {
                    var gf = new VsGitFlowWrapper(GitFlowPage.ActiveRepoPath, GitFlowPage.OutputWindow);
                    return gf.CurrentBranchLeafName;
                }
                return "No branch selected";
            }
            set { ; } }


        public GitFlowPageModel()
        {
            SelectBranchCommand = new RelayCommand(p => SelectBranch(), p => CanSelectBranch);

        }

        private void SelectBranch()
        {
            GitFlowPage.ShowPage(TeamExplorerPageIds.GitBranches);
        }

        public bool CanSelectBranch
        {
            get { return true; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            OnPropertyChanged("CurrentBranch");
        }
    }
}