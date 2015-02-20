using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),"git")))
            {
                GitInstallation.Visibility = Visibility.Visible;
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
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        WorkingDirectory = Path.Combine(installationPath, "Dependencies"),
                        UseShellExecute = true,
                        Arguments = "-ExecutionPolicy ByPass -NoLogo -NoProfile -File \"" + cmd + "\"",
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
