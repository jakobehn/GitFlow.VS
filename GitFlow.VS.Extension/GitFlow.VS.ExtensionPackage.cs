using System.Runtime.InteropServices;
using Microsoft.ApplicationInsights;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using TeamExplorer.Common;

namespace GitFlowVS.Extension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [Guid(GuidList.GuidGitFlowVsExtensionPkgString)]
    public sealed class GitFlowVSExtension : Package
    {
        public GitFlowVSExtension()
        {
            
        }
        protected override void Initialize()
        {
            base.Initialize();
            UserSettings.ServiceProvider = this;
        }
    }
}
