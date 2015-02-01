using GitFlow.VS;
using Microsoft.VisualStudio.Shell.Interop;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    public class VsGitFlowWrapper : GitFlowWrapper
    {
        public VsGitFlowWrapper(string repoPath,IVsOutputWindowPane outputWindow)
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
                outputWindow.OutputStringThreadSafe(args.Output);
            };
        }
    }
}