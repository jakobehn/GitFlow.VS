using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.Forms.MessageBox;
using Path = System.IO.Path;

namespace GitFlowVS.Extension.UI
{
    /// <summary>
    /// Interaction logic for InstallGitFlowUI.xaml
    /// </summary>
    public partial class InstallGitFlowUI : UserControl
    {
        private readonly GitFlowSection parent;

        public InstallGitFlowUI(GitFlowSection parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                    //parent.FinishAction();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
