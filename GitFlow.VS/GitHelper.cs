using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;

namespace GitFlow.VS
{
    public class GitHelper
    {
        public static string GetGitBinPath()
        {
            var installationPath = GetGitInstallationPath();
            if (installationPath == null)
                return null;

            var binPath = Path.Combine(installationPath, "usr/bin");
            if (Directory.Exists(binPath))
            {
                return binPath;
            }
            return Path.Combine(installationPath, "bin");
        }

        public static string GetGitInstallationPath()
        {
            string gitPath = GetInstallPathFromEnvironmentVariable();
            if (gitPath != null)
                return gitPath;

            gitPath = GetInstallPathFromRegistry();
            if (gitPath != null)
                return gitPath;

            gitPath = GetInstallPathFromProgramFiles();
            if (gitPath != null)
                return gitPath;
            return null;
        }

        public static string GetInstallPathFromEnvironmentVariable()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            var allPaths = path.Split(';');
            string gitPath = allPaths.FirstOrDefault(p => p.ToLower().TrimEnd('\\').EndsWith("git\\cmd"));
            if (gitPath != null && Directory.Exists(gitPath))
            {
                gitPath = Directory.GetParent(gitPath).FullName.TrimEnd('\\');
            }
            return gitPath;
        }

        public static string GetInstallPathFromRegistry()
        {
            //try 64-bit OS
            var installLocation = Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows", "InstallPath", null);
            if (installLocation != null && Directory.Exists(installLocation.ToString().TrimEnd('\\')))
                return installLocation.ToString().TrimEnd('\\');

            //try 32-bit OS
            installLocation = Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows", "InstallPath", null);
            if (installLocation != null && Directory.Exists(installLocation.ToString().TrimEnd('\\')))
                return installLocation.ToString().TrimEnd('\\');
            return null;
        }

        public static string GetInstallPathFromProgramFiles()
        {
            string gitPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "git");
            if (Directory.Exists(gitPath))
                return gitPath.TrimEnd('\\');
            gitPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "git");
            if (Directory.Exists(gitPath))
                return gitPath.TrimEnd('\\');
            return null;
        }
    }
}