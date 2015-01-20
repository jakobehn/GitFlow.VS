using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace GitFlow.VS
{
    public class GitFlowWrapper
    {
        public static StringBuilder Output = new StringBuilder("");
        public static StringBuilder Error = new StringBuilder("");
        private const string GitFlowDefaultValueRegExp = @"\[(.*?)\]";

        public void StartFeature(string repoDirectory, string featureName)
        {
            string gitArguments = "feature start \"" + featureName + "\"";
            RunGit(repoDirectory, gitArguments);
        }

        public void FinishFeature(string repoDirectory, string featureName)
        {
            string gitArguments = "feature finish \"" + featureName + "\"";
            RunGit(repoDirectory, gitArguments);
        }

        public void StartRelease(string repoDirectory, string releaseName)
        {
            string gitArguments = "release start \"" + releaseName + "\"";
            RunGit(repoDirectory, gitArguments);
        }

        public void FinishRelease(string repoDirectory, string releaseName)
        {
            string gitArguments = "release finish -n \"" + releaseName + "\"";
            RunGit(repoDirectory, gitArguments);
        }

        public void StartHotfix(string repoDirectory, string hotfixName)
        {
            string gitArguments = "hotfix start \"" + hotfixName + "\"";
            RunGit(repoDirectory, gitArguments);
        }

        public void FinishHotfix(string repoDirectory, string hotfixName)
        {
            string gitArguments = "hotfix finish -n \"" + hotfixName + "\"";
            RunGit(repoDirectory, gitArguments);
        }

        public GitFlowCommandResult Init(string repoDirectory, GitFlowRepoSettings settings)
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
                        Output.Append(input);
                        var line = input.ToString();
                        input = new StringBuilder();
                        Debug.WriteLine(line);
                    }
                    if (IsMasterBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.MasterBranch);
                        Output.Append(input);
                        input = new StringBuilder();
                    }
                    else if (IsDevelopBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.DevelopBranch);
                        Output.Append(input);
                        input = new StringBuilder();
                    }
                    else if (IsFeatureBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.FeatureBranch);
                        Output.Append(input);
                        input = new StringBuilder();
                    }
                    else if (IsReleaseBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.ReleaseBranch);
                        Output.Append(input);
                        input = new StringBuilder();
                    }
                    else if (IsHotfixBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.HotfixBranch);
                        Output.Append(input);
                        input = new StringBuilder();
                    }
                    else if (IsSupportBranchQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.SupportBranch);
                        Output.Append(input);
                        input = new StringBuilder();
                    }
                    else if (IsVersionTagPrefixQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine(settings.VersionTag);
                        Output.Append(input);
                        input = new StringBuilder();
                    }
                    else if (IsHooksAndFiltersQuery(input.ToString()))
                    {
                        p.StandardInput.WriteLine("");
                        Output.Append(input);
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
            return new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "git",
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
            }
        }
        private void OnErrorReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            Error = new StringBuilder();
            Error.Append(dataReceivedEventArgs.Data);
            Debug.WriteLine(dataReceivedEventArgs.Data);
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

        private void RunGit(string repoDirectory, string gitArguments)
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
                    throw new Exception(Error.ToString());
                }
            }
        }
    }

}
