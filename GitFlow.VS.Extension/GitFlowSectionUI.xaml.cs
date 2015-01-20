using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GitFlow.VS;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using MessageBox = System.Windows.Forms.MessageBox;

namespace JakobEhn.GitFlow_VS_Extension
{
    /// <summary>
    /// Interaction logic for GitFlowSectionUI.xaml
    /// </summary>
    public partial class GitFlowSectionUI : UserControl
    {
        public GitFlowSectionUI()
        {
            InitializeComponent();
        }

        public IGitRepositoryInfo ActiveRepo { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                CommandOutput.Clear();
                using (new WaitCursor())
                {
                    var gf = new GitFlowWrapper(ActiveRepo.RepositoryPath);
                    gf.CommandOutputDataReceived += (o, args) =>
                    {
                        CommandOutput.AppendText(args.Output);
                    };
                    gf.Init(new GitFlowRepoSettings());
                }
            }
        }

        private void StartFeature_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ActiveRepo != null)
                {
                    CommandOutput.Clear();
                    using (new WaitCursor())
                    {
                        var gf = new GitFlowWrapper(ActiveRepo.RepositoryPath);
                        //gf.CommandOutputDataReceived += (o, args) =>
                        //{
                        //    if (args != null && args.Output != null)
                        //    {
                        //        Dispatcher.Invoke((Action)delegate()
                        //        {
                        //            CommandOutput.AppendText(args.Output);
                        //        });

                        //    }
                        //};
                        var result = gf.StartFeature(FeatureName.Text);
                        CommandOutput.AppendText(result.CommandOutput);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.ToString());
            }
        }

        private void FinishFeature_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRepo != null)
            {
                CommandOutput.Clear();
                using (new WaitCursor())
                {
                    var gf = new GitFlowWrapper(ActiveRepo.RepositoryPath);
                    var result = gf.FinishFeature(FeatureName.Text);
                    CommandOutput.AppendText(result.CommandOutput);
                }
            }
        }
    }
}
