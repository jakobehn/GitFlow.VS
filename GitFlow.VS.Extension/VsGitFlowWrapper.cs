using System.Windows.Forms;
using GitFlow.VS;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    public class VsGitFlowWrapper : GitFlowWrapper
    {
        public VsGitFlowWrapper(string repoPath,IVsOutputWindowPane outputWindow, TeamExplorerBaseSection section)
            : base(repoPath)
        {
            CommandOutputDataReceived += (o, args) =>
            {
                outputWindow.OutputStringThreadSafe(args.Output);
            };
            CommandErrorDataReceived += (o, args) =>
            {
                //section.ShowNotification(args.Output, NotificationType.Error);
                outputWindow.OutputStringThreadSafe(args.Output);
            };
        }
    }
}