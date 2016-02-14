using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace GitFlow.VS.Tests
{
    [TestClass]
    public class GitTests
    {
        private const string InstallPath = @"c:\program files (x86)\git";
        private const string InstallPath64 = @"c:\program files\git";

        [TestMethod]
        [Ignore]
        public void CheckInstallationPathFromEnvironment()
        {
            var installPath = GitHelper.GetInstallPathFromEnvironmentVariable().ToLower();
            Assert.IsTrue(installPath == InstallPath || installPath == InstallPath64);
        }

        [TestMethod]
        [Ignore]
        public void CheckInstallationPathFromRegistry()
        {
            var installPath = GitHelper.GetInstallPathFromRegistry().ToLower();
            Assert.IsTrue(installPath == InstallPath || installPath == InstallPath64);
        }

        [TestMethod]
        [Ignore]
        public void CheckInstallationPathFromProgramFiles()
        {
            var installPath = GitHelper.GetInstallPathFromProgramFiles().ToLower();
            Assert.IsTrue(installPath == InstallPath || installPath == InstallPath64);
        }


  
    }
}
