using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using TeamExplorer.Common;
using System;
using System.Threading;

namespace GitFlowVS.Extension
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(GuidList.GuidGitFlowVsExtensionPkgString)]
    public sealed class GitFlowVSExtension : AsyncPackage
    {
        public GitFlowVSExtension()
        {
        }

        protected override System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            UserSettings.ServiceProvider = this;
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }
    }
}
