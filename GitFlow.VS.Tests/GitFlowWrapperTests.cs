using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitFlow.VS.Tests
{
    [TestClass]
    public class GitFlowWrapperTests
    {
        private string sampleRepoPath;
        private const string SampleGitRepoName = "SampleGitRepo";

        public GitFlowWrapperTests()
        {
            sampleRepoPath = Path.Combine(Directory.GetCurrentDirectory(), SampleGitRepoName);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (Directory.Exists(SampleGitRepoName))
            {
                DeleteReadOnlyDirectory(SampleGitRepoName);
            }

            ZipFile.ExtractToDirectory("TestData\\SampleGitRepo.zip", ".");
        }

        [TestMethod]
        public void MakeSureTestRepoIsCopied()
        {
            Assert.IsTrue(File.Exists("TestData\\SampleGitRepo.zip"));
        }

        [TestMethod]
        public void MakeSureTestRepoIsExtracted()
        {
            Assert.IsTrue(Directory.Exists(SampleGitRepoName));
        }

        [TestMethod]
        public void Init()
        {
            var gf = new GitFlowWrapper();
            var result = gf.Init(sampleRepoPath, new GitFlowRepoSettings());
            Assert.IsTrue(result.Success);
            Debug.Write(result.CommandOutput);

            using (var repo = new Repository(sampleRepoPath))
            {
                var branches = repo.Branches.Where(b => !b.IsRemote);
                Assert.AreEqual(2, branches.Count());
                Assert.IsTrue(branches.Any(b => b.Name == "master"));
                Assert.IsTrue(branches.Any(b => b.Name == "develop"));
            }
        }

        [TestMethod]
        public void StartFeature()
        {
            var gf = new GitFlowWrapper();
            gf.Init(sampleRepoPath, new GitFlowRepoSettings());
            gf.StartFeature(sampleRepoPath, "X");

            using (var repo = new Repository(sampleRepoPath))
            {
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "feature/X"));
            }
        }

        [TestMethod]
        public void FinishFeature()
        {
            var gf = new GitFlowWrapper();
            gf.Init(sampleRepoPath, new GitFlowRepoSettings());
            gf.StartFeature(sampleRepoPath, "X");
            gf.FinishFeature(sampleRepoPath, "X");
            using (var repo = new Repository(sampleRepoPath))
            {
                //Feature branch should be deleted (default option)
                Assert.IsFalse(repo.Branches.Any(b => !b.IsRemote && b.Name == "feature/X"));
            }
        }

        [TestMethod]
        public void StartRelease()
        {
            var gf = new GitFlowWrapper();
            gf.Init(sampleRepoPath, new GitFlowRepoSettings());
            gf.StartRelease(sampleRepoPath, "1.0");

            using (var repo = new Repository(sampleRepoPath))
            {
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "release/1.0"));
            }
        }

        [TestMethod]
        public void FinishRelease()
        {
            var gf = new GitFlowWrapper();
            gf.Init(sampleRepoPath, new GitFlowRepoSettings());
            gf.StartRelease(sampleRepoPath, "1.0");
            gf.FinishRelease(sampleRepoPath, "1.0");
            using (var repo = new Repository(sampleRepoPath))
            {
                //Release branch should be deleted (default option)
                Assert.IsFalse(repo.Branches.Any(b => !b.IsRemote && b.Name == "release/1.0"));
            }
        }

        [TestMethod]
        public void StartHotfix()
        {
            var gf = new GitFlowWrapper();
            gf.Init(sampleRepoPath, new GitFlowRepoSettings());
            gf.StartHotfix(sampleRepoPath, "hf1");

            using (var repo = new Repository(sampleRepoPath))
            {
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "hotfix/hf1"));
            }
        }

        [TestMethod]
        public void FinishHotfix()
        {
            var gf = new GitFlowWrapper();
            gf.Init(sampleRepoPath, new GitFlowRepoSettings());
            gf.StartHotfix(sampleRepoPath, "hf1");
            gf.FinishHotfix(sampleRepoPath, "hf1");
            using (var repo = new Repository(sampleRepoPath))
            {
                //Hotfix branch should be deleted (default option)
                Assert.IsFalse(repo.Branches.Any(b => !b.IsRemote && b.Name == "hotfix/hf1"));
            }
        }

        [TestMethod]
        public void GitFlowInitWithInvalidMasterBranchShouldFail()
        {
            try
            {
                var gf = new GitFlowWrapper();
                gf.Init(sampleRepoPath, new GitFlowRepoSettings() {MasterBranch = "master2"});
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("master2"));
            }
        }

        [TestMethod]
        public void ParseMasterBranchQuery()
        {
            var gf = new GitFlowWrapper();
            string query = "Branch name for production releases: [dev]";
            Assert.IsTrue(gf.IsMasterBranchQuery(query));
        }

        [TestMethod]
        public void ParseFeatureBranchQuery()
        {
            var gf = new GitFlowWrapper();
            string query = "Feature branches? [feature/]";
            Assert.IsTrue(gf.IsFeatureBranchQuery(query));
        }

        [TestMethod]
        public void ParseVersionTagPrefixQuery()
        {
            var gf = new GitFlowWrapper();
            string query = "Version tag prefix? [v]";
            Assert.IsTrue(gf.IsVersionTagPrefixQuery(query));
        }

        [TestMethod]
        public void ParseVersionTagPrefixQueryWithEmptyTag()
        {
            var gf = new GitFlowWrapper();
            string query = "Version tag prefix? []";
            Assert.IsTrue(gf.IsVersionTagPrefixQuery(query));
        }

        [TestMethod]
        public void ParseHooksAndFiltersQuery()
        {
            var gf = new GitFlowWrapper();
            string query = "Hooks and filters directory? [c:/Users/jakobe/Source/Repos/GitFlowTest/.git/hooks]";
            Assert.IsTrue(gf.IsHooksAndFiltersQuery(query));
        }

        //Necessary in order to delete directory containing readonly files
        public static void DeleteReadOnlyDirectory(string directory)
        {
            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                DeleteReadOnlyDirectory(subdirectory);
            }
            foreach (var fileName in Directory.EnumerateFiles(directory))
            {
                var fileInfo = new FileInfo(fileName) {Attributes = FileAttributes.Normal};
                fileInfo.Delete();
            }
            Directory.Delete(directory);
        }
    }


}
