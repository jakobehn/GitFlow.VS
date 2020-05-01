namespace GitFlow.VS
{
    public class GitFlowRepoSettings
    {
        public string MasterBranch { get; set; }
        public string DevelopBranch { get; set; }
        public string FeatureBranch { get; set; }
        public string BugfixBranch { get; set; }
        public string ReleaseBranch { get; set; }
        public string HotfixBranch { get; set; }
        public string SupportBranch { get; set; }
        public string VersionTag { get; set; }

        public GitFlowRepoSettings()
        {
            MasterBranch = "master";
            DevelopBranch = "develop";
            FeatureBranch = "feature/";
            BugfixBranch = "bugfix/";
            ReleaseBranch = "release/";
            HotfixBranch = "hotfix/";
            SupportBranch = "support/";
            VersionTag = "";
        }
    }
}