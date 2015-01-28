using System;
using System.Windows.Forms;
using System.Windows.Threading;
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
                if (!String.IsNullOrEmpty(args.Output) && args.Output != Environment.NewLine)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() => section.ShowNotification(args.Output.Trim(), NotificationType.Error)));
                    outputWindow.OutputStringThreadSafe(args.Output.Trim());
                }
            };
        }
    }
}