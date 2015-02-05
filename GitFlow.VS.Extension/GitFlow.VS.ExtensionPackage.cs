using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace GitFlowVS.Extension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [Guid(GuidList.GuidGitFlowVsExtensionPkgString)]
    public sealed class GitFlow_VS_ExtensionPackage : Package
    {
    }
}
