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

        [TestMethod]
        public void CheckInstallationPathFromEnvironment()
        {
            Assert.AreEqual(InstallPath, GitHelper.GetInstallPathFromEnvironmentVariable().ToLower());
        }

        [TestMethod]
        public void CheckInstallationPathFromRegistry()
        {
            Assert.AreEqual(InstallPath, GitHelper.GetInstallPathFromRegistry().ToLower());
        }

        [TestMethod]
        public void CheckInstallationPathFromProgramFiles()
        {
            Assert.AreEqual(InstallPath, GitHelper.GetInstallPathFromProgramFiles().ToLower());
        }


  
    }
}
