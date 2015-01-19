using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
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

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
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
        public void GitFlowInit()
        {
            
            var gf = new GitFlowWrapper();
            gf.Init(sampleRepoPath, new GitFlowRepoSettings());
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
