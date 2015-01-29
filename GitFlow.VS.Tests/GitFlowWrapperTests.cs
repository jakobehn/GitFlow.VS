using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitFlow.VS.Tests
{
    [TestClass]
    public class GitFlowWrapperTests
    {
        private readonly string sampleRepoPath;
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
        public void RunInstallScript()
        {
            string installationPath =
                @"C:\Users\jakobe\AppData\Local\Microsoft\VisualStudio\12.0Exp\Extensions\Jakob Ehn\GitFlow.VS\0.9";
            string cmd = Path.Combine(installationPath, "Dependencies\\install.ps1");
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    WorkingDirectory = Path.Combine(installationPath, "Dependencies"),
                    UseShellExecute = true,
                    Arguments = @"-File ""C:\Users\jakobe\Source\Repos\GitFlow.VS\GitFlow.VS.Extension\Dependencies\Install.ps1""",
                    Verb = "runas",
                    LoadUserProfile = false
                }
            };
            proc.Start();
            proc.WaitForExit();
            var exit = proc.ExitCode;
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
        public void StartFeatureShouldPutUsOnFeatureBranch()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartFeature("X");

            Assert.IsTrue(gf.IsOnFeatureBranch);
        }

        [TestMethod]
        public void StartReleaseShouldPutUsOnReleaseBranch()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartRelease("2.0");

            Assert.IsTrue(gf.IsOnReleaseBranch);
        }

        [TestMethod]
        public void StartHotfixShouldPutUsOnHotfixBranch()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartHotfix("hf1");

            Assert.IsTrue(gf.IsOnHotfixBranch);
        }

        [TestMethod]
        public void CheckNotInitialized()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            Assert.IsFalse(gf.IsInitialized);
            gf.Init(new GitFlowRepoSettings());
            Assert.IsTrue(gf.IsInitialized);
        }

        [TestMethod]
        public void Init()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.CommandOutputDataReceived += (sender, args) => { Console.Write(args.Output); };
            var result = gf.Init(new GitFlowRepoSettings());
            Assert.IsTrue(result.Success);

            Assert.IsTrue(gf.IsOnDevelopBranch);
            using (var repo = new Repository(sampleRepoPath))
            {
                var branches = repo.Branches.Where(b => !b.IsRemote).ToList();
                Assert.AreEqual(2, branches.Count());
                Assert.IsTrue(branches.Any(b => b.Name == "master"));
                Assert.IsTrue(branches.Any(b => b.Name == "develop"));
            }
        }

        [TestMethod]
        public void StartFeature()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartFeature("X");

            using (var repo = new Repository(sampleRepoPath))
            {
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "feature/X"));
            }
        }

        [TestMethod]
        public void GetStatus()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartFeature("X");
            Assert.AreEqual("X", gf.CurrentBranchLeafName);
            Assert.AreEqual(gf.CurrentStatus, "Feature: X");
        }

        [TestMethod]
        public void StartTwoFeaturesInParallel()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartFeature("X");
            gf.StartFeature("Y");

            using (var repo = new Repository(sampleRepoPath))
            {
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "feature/X"));
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "feature/Y"));
            }
        }

        [TestMethod]
        public void FinishFeature()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartFeature("X");
            gf.FinishFeature("X");
            using (var repo = new Repository(sampleRepoPath))
            {
                //Feature branch should be deleted (default option)
                Assert.IsFalse(repo.Branches.Any(b => !b.IsRemote && b.Name == "feature/X"));
            }
        }

        [TestMethod]
        public void StartRelease()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartRelease("1.0");

            using (var repo = new Repository(sampleRepoPath))
            {
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "release/1.0"));
            }
        }

        [TestMethod]
        public void FinishRelease()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartRelease("1.0");
            gf.FinishRelease("1.0");
            using (var repo = new Repository(sampleRepoPath))
            {
                //Release branch should be deleted (default option)
                Assert.IsFalse(repo.Branches.Any(b => !b.IsRemote && b.Name == "release/1.0"));
            }
        }

        [TestMethod]
        public void StartHotfix()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartHotfix("hf1");

            using (var repo = new Repository(sampleRepoPath))
            {
                Assert.IsTrue(repo.Branches.Any(b => !b.IsRemote && b.Name == "hotfix/hf1"));
            }
        }

        [TestMethod]
        public void FinishHotfix()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            gf.Init(new GitFlowRepoSettings());
            gf.StartHotfix("hf1");
            gf.FinishHotfix("hf1");
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
                var gf = new GitFlowWrapper(sampleRepoPath);
                gf.Init(new GitFlowRepoSettings {MasterBranch = "master2"});
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("master2"));
            }
        }

        [TestMethod]
        public void ParseMasterBranchQuery()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            const string query = "Branch name for production releases: [dev]";
            Assert.IsTrue(gf.IsMasterBranchQuery(query));
        }

        [TestMethod]
        public void ParseFeatureBranchQuery()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            const string query = "Feature branches? [feature/]";
            Assert.IsTrue(gf.IsFeatureBranchQuery(query));
        }

        [TestMethod]
        public void ParseVersionTagPrefixQuery()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            const string query = "Version tag prefix? [v]";
            Assert.IsTrue(gf.IsVersionTagPrefixQuery(query));
        }

        [TestMethod]
        public void ParseVersionTagPrefixQueryWithEmptyTag()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            const string query = "Version tag prefix? []";
            Assert.IsTrue(gf.IsVersionTagPrefixQuery(query));
        }

        [TestMethod]
        public void ParseHooksAndFiltersQuery()
        {
            var gf = new GitFlowWrapper(sampleRepoPath);
            const string query = "Hooks and filters directory? [c:/Users/jakobe/Source/Repos/GitFlowTest/.git/hooks]";
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
