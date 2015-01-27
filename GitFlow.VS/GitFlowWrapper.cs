using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LibGit2Sharp;

namespace GitFlow.VS
{
    public class CommandOutputEventArgs : EventArgs
    {
        public CommandOutputEventArgs()
        {
            
        }

        public CommandOutputEventArgs(string output)
        {
            Output = output;
        }
        public string Output { get; set; }
    }

    public class GitFlowWrapper
    {
        public delegate void CommandOutputReceivedEventHandler(object sender, CommandOutputEventArgs args);
        public delegate void CommandErrorReceivedEventHandler(object sender, CommandOutputEventArgs args);

        private readonly string repoDirectory;
        public static StringBuilder Output = new StringBuilder("");
        public static StringBuilder Error = new StringBuilder("");
        private const string GitFlowDefaultValueRegExp = @"\[(.*?)\]";

        public event CommandOutputReceivedEventHandler CommandOutputDataReceived;
        public event CommandErrorReceivedEventHandler CommandErrorDataReceived;

        public bool IsOnFeatureBranch
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    var featurePrefix = repo.Config.Get<string>("gitflow.prefix.feature");
                    return repo.Head.Name.StartsWith(featurePrefix.Value);
                }
            }
        }

        public bool IsOnHotfixBranch
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    var hotfixPrefix = repo.Config.Get<string>("gitflow.prefix.hotfix");
                    return repo.Head.Name.StartsWith(hotfixPrefix.Value);
                }
            }
        }

        public bool IsOnMasterBranch
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    var masterBranch = repo.Config.Get<string>("gitflow.branch.master");
                    return repo.Head.Name == masterBranch.Value;
                }
            }
        }

        public string CurrentStatus
        {
            get
            {
                string status = "";
                using (var repo = new Repository(repoDirectory))
                {
                    if (IsOnDevelopBranch)
                        status = "Develop: " + CurrentBranchLeafName;
                    else if (IsOnFeatureBranch)
                        status = "Feature: " + CurrentBranchLeafName;
                    else if (IsOnHotfixBranch)
                        status = "Hotfix: " + CurrentBranchLeafName;
                }
                return status;
            }
        }

        public bool IsOnDevelopBranch
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    var developBranch = repo.Config.Get<string>("gitflow.branch.develop");
                    return repo.Head.Name == developBranch.Value;
                }
            }
        }

        public bool IsOnReleaseBranch
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    var releasePrefix = repo.Config.Get<string>("gitflow.prefix.release");
                    return repo.Head.Name.StartsWith(releasePrefix.Value);
                }
            }
        }

        public string CurrentBranch
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    return repo.Head.Name;
                }                
            }
        }

        public string CurrentBranchLeafName
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    string fullBranchName = repo.Head.Name;
                    var parts = fullBranchName.Split('/');
                    return parts.Last();
                }
            }
        }


        protected virtual void OnCommandOutputDataReceived(CommandOutputEventArgs e)
        {
            CommandOutputReceivedEventHandler handler = CommandOutputDataReceived;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnCommandErrorDataReceived(CommandOutputEventArgs e)
        {
            CommandErrorReceivedEventHandler handler = CommandErrorDataReceived;
            if (handler != null) handler(this, e);
        }

        public GitFlowWrapper(string repoDirectory)
        {
            this.repoDirectory = repoDirectory;
        }

        public GitFlowCommandResult StartFeature(string featureName)
        {
            string gitArguments = "feature start \"" + featureName + "\"";
            return RunGitFlow(gitArguments);
        }

        public GitFlowCommandResult FinishFeature(string featureName, bool rebaseOnDevelopment = false, bool deleteBranch = true)
        {
            string gitArguments = "feature finish \"" + featureName + "\"";
            if (rebaseOnDevelopment)
                gitArguments += " -r";
            if (!deleteBranch)
                gitArguments += " -k";

            return RunGitFlow(gitArguments);

        }

        public GitFlowCommandResult StartRelease(string releaseName)
        {
            string gitArguments = "release start \"" + releaseName + "\"";
            return RunGitFlow(gitArguments);
        }

        public GitFlowCommandResult FinishRelease(string releaseName, string tagMessage = null, bool deleteBranch = true, bool forceDeletion=false, bool pushChanges = false)
        {
            string gitArguments = "release finish -n \"" + releaseName + "\"";
            if (!String.IsNullOrEmpty(tagMessage))
            {
                gitArguments += " -m  + \"" + tagMessage + "\"";
            }
            if (!deleteBranch)
            {
                gitArguments += " -k";
                if (forceDeletion)
                {
                    gitArguments += " -D";
                }
            }
            if (pushChanges)
            {
                gitArguments += " -p";
            }

            return RunGitFlow(gitArguments);
        }

        public GitFlowCommandResult StartHotfix(string hotfixName)
        {
            string gitArguments = "hotfix start \"" + hotfixName + "\"";
            return RunGitFlow(gitArguments);
        }

        public GitFlowCommandResult FinishHotfix(string hotfixName)
        {
            string gitArguments = "hotfix finish -n \"" + hotfixName + "\"";
            return RunGitFlow(gitArguments);
        }

        public GitFlowCommandResult Init(GitFlowRepoSettings settings)
        {
            Error = new StringBuilder("");
            Output = new StringBuilder("");

            using (var p = CreateGitFlowProcess("init -f", repoDirectory))
            {
                p.Start();
                p.ErrorDataReceived += OnErrorReceived;
                p.BeginErrorReadLine();
                var input = new StringBuilder();

                var sr = p.StandardOutput;
                while (!sr.EndOfStream)
                {
                    var inputChar = (char) sr.Read();
                    input.Append(inputChar);
                    if (StringBuilderEndsWith(input, Environment.NewLine))
                    {
                        Output.AppendLine(input.ToString());
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input.ToString()));
                        input = new StringBuilder();
                    }
                    if (IsMasterBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.MasterBranch);
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                    else if (IsDevelopBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.DevelopBranch);
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                    else if (IsFeatureBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.FeatureBranch);
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                    else if (IsReleaseBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.ReleaseBranch);
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                    else if (IsHotfixBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.HotfixBranch);
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                    else if (IsSupportBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.SupportBranch);
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                    else if (IsVersionTagPrefixQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.VersionTag);
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                    else if (IsHooksAndFiltersQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine("");
                        Output.Append(input);
                        OnCommandOutputDataReceived(new CommandOutputEventArgs(input + Environment.NewLine));
                        input = new StringBuilder();
                    }
                }
            }
            if (Error != null && Error.Length > 0)
            {
                return new GitFlowCommandResult(false, Error.ToString());
            }
            return new GitFlowCommandResult(true, Output.ToString());
        }

        private static Process CreateGitFlowProcess(string arguments, string repoDirectory)
        {
            string pathToGit = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),@"git\bin\git.exe");
            return new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = pathToGit,
                    Arguments = "flow " + arguments,
                    WorkingDirectory = repoDirectory
                }
            };
        }


        private void OnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs.Data != null)
            {
                Output.Append(dataReceivedEventArgs.Data);
                Debug.WriteLine(dataReceivedEventArgs.Data);
                OnCommandOutputDataReceived(new CommandOutputEventArgs(dataReceivedEventArgs.Data + Environment.NewLine));
            }
        }
        private void OnErrorReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            Error = new StringBuilder();
            Error.Append(dataReceivedEventArgs.Data);
            Debug.WriteLine(dataReceivedEventArgs.Data);
            OnCommandErrorDataReceived(new CommandOutputEventArgs(dataReceivedEventArgs.Data + Environment.NewLine));
        }

        public bool IsMasterBranchQuery(string input)
        {
            var regex = new Regex(@"Branch name for production releases: " + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }

        public bool IsDevelopBranchQuery(string input)
        {
            var regex = new Regex(@"Branch name for ""next release"" development: " + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }

        public bool IsFeatureBranchQuery(string input)
        {
            var regex = new Regex(@"Feature branches\? " + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }

        public bool IsReleaseBranchQuery(string input)
        {
            var regex = new Regex(@"Release branches\? " + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }
        public bool IsHotfixBranchQuery(string input)
        {
            var regex = new Regex(@"Hotfix branches\? " + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }

        public bool IsSupportBranchQuery(string input)
        {
            var regex = new Regex(@"Support branches\? "  + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }

        public bool IsVersionTagPrefixQuery(string input)
        {
            var regex = new Regex(@"Version tag prefix\? " + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }

        public bool IsHooksAndFiltersQuery(string input)
        {
            var regex = new Regex(@"Hooks and filters directory\? " + GitFlowDefaultValueRegExp);
            return MatchInput(input, regex);
        }

        private static bool MatchInput(string input, Regex regex)
        {
            var match = regex.Match(input);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        private static bool StringBuilderEndsWith(StringBuilder haystack, string needle)
        {
            if (haystack.Length == 0)
                return false;

            var needleLength = needle.Length - 1;
            var haystackLength = haystack.Length - 1;
            for (var i = 0; i < needleLength; i++)
            {
                if (haystack[haystackLength - i] != needle[needleLength - i])
                {
                    return false;
                }
            }
            return true;
        }

        private GitFlowCommandResult RunGitFlow(string gitArguments)
        {
            Error = new StringBuilder("");
            Output = new StringBuilder("");

            using (var p = CreateGitFlowProcess(gitArguments, repoDirectory))
            {
                p.Start();
                p.ErrorDataReceived += OnErrorReceived;
                p.OutputDataReceived += OnOutputDataReceived;
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();
                p.WaitForExit();

                if (Error != null && Error.Length > 0)
                {
                    return new GitFlowCommandResult(false, Error.ToString());
                }
                return new GitFlowCommandResult(true, Output.ToString());
            }
        }
    }

}
