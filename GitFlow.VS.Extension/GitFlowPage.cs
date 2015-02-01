using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [TeamExplorerPage(GuidList.gitFlowPage, Undockable = true)]
    public class GitFlowPage : TeamExplorerBasePage
    {
        private static IGitExt gitService;
        private static IVsOutputWindowPane outputWindow;

        public static IGitRepositoryInfo ActiveRepo
        {
            get
            {
                return gitService.ActiveRepositories.FirstOrDefault();
            }
        }

        public static IVsOutputWindowPane OutputWindow
        {
            get { return outputWindow; }
        }

        public static string ActiveRepoPath
        {
            get { return ActiveRepo.RepositoryPath; }
        }

        [ImportingConstructor]
        public GitFlowPage([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            Title = "GitFlow";

            gitService = (IGitExt)serviceProvider.GetService(typeof(IGitExt));
            
            var outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            var customGuid = new Guid("B85225F6-B15E-4A8A-AF6E-2BE96A4FE672");
            outWindow.CreatePane(ref customGuid, "GitFlow.VS", 1, 1);
            outWindow.GetPane(ref customGuid, out outputWindow);
        }

        public static void ActiveOutputWindow()
        {
            OutputWindow.Activate();
        }
    }

}
