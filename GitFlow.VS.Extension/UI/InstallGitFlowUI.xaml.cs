using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GitFlow.VS;
using Microsoft.TeamFoundation.Controls;
using Path = System.IO.Path;

namespace GitFlowVS.Extension.UI
{
    /// <summary>
    /// Interaction logic for InstallGitFlowUI.xaml
    /// </summary>
    public partial class InstallGitFlowUI : UserControl
    {
        private readonly GitFlowInstallSection parent;

        public InstallGitFlowUI(GitFlowInstallSection parent)
        {
			Logger.PageView("InstallGitFlow");
            this.parent = parent;
            InitializeComponent();

            if (GitHelper.GetGitInstallationPath() == null)
            {
                GitInstallation.Visibility = Visibility.Visible;
                GitFlowInstallation.Visibility = Visibility.Collapsed;
                GitFlowInstallationButton.Visibility = Visibility.Collapsed;
                GitNoRepo.Visibility = Visibility.Collapsed;
            }
            else if(GitFlowPage.ActiveRepo == null)
            {
                GitNoRepo.Visibility = Visibility.Visible;
                GitInstallation.Visibility = Visibility.Collapsed;
                GitFlowInstallation.Visibility = Visibility.Collapsed;
                GitFlowInstallationButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
	            Logger.Event("Install");
                Error.Visibility = Visibility.Hidden;
                var installationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string cmd = Path.Combine(installationPath, "Dependencies\\install.ps1");

                var gitInstallPath = GitHelper.GetGitInstallationPath();
                if (Directory.Exists(Path.Combine(gitInstallPath, "usr")))
                {
                    gitInstallPath = Path.Combine(gitInstallPath, "usr");
                }

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        WorkingDirectory = Path.Combine(installationPath, "Dependencies"),
                        UseShellExecute = true,
                        Arguments = String.Format("-ExecutionPolicy ByPass -NoLogo -NoProfile -File \"" + cmd + "\" \"{0}\"", gitInstallPath),
                        Verb = "runas",
                        LoadUserProfile = false
                    }
                };
                proc.Start();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    Error.Text = Error.Text.Replace("{0}", proc.ExitCode.ToString());
                    Error.Visibility = Visibility.Visible;
                }
                else
                {
                    parent.Refresh();
                }
            }
            catch (Exception ex)
            {
                parent.ShowNotification(ex.ToString(), NotificationType.Error);
				Logger.Exception(ex);
            }
        }
    }
}
