using System;
using System.Windows.Forms;
using System.Windows.Threading;
using GitFlow.VS;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using TeamExplorer.Common;
using Application = System.Windows.Application;

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
                //if (!String.IsNullOrEmpty(args.Output) && args.Output != Environment.NewLine)
                //{
                //    Application.Current.Dispatcher.BeginInvoke(
                //        DispatcherPriority.Background,
                //        new Action(() => section.ShowNotification(args.Output.Trim(), NotificationType.Error)));
                //    outputWindow.OutputStringThreadSafe(args.Output.Trim());
                //}
                outputWindow.OutputStringThreadSafe(args.Output.Trim());
            };
        }
    }
}