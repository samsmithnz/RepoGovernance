using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using RepoGovernance.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoGovernance.Tests;

[TestClass]
public class GitHubAPIAccessTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetRepoTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "TBS";

        //Act
        Repo? repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret,
            owner, repository);

        //Assert
        Assert.IsNotNull(repo);
    }

    [TestMethod]
    public async Task GetReposTest()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        List<Repo> repos = await GitHubAPIAccess.GetRepos(base.GitHubId, base.GitHubSecret,
              owner);

        //Assert
        Assert.IsNotNull(repos);
        Assert.IsTrue(repos.Count >= 30);
        //bool foundPublicRepo = false;
        //bool foundPrivateRepo = false;
        //foreach (Repo repo in repos)
        //{
        //    if (repo.visibility == "public")
        //    {
        //        foundPublicRepo = true;
        //    }
        //    else if (repo.visibility == "private")
        //    {
        //        foundPrivateRepo = true;
        //    }
        //}
        //Assert.IsTrue(foundPublicRepo);
        //Assert.IsTrue(foundPrivateRepo);
    }

    [TestMethod]
    public async Task GetFilesTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repo = "RepoGovernance";
        string path = ".github/workflows";

        //Act
        GitHubFile[]? files = await RepoAutomation.Core.APIAccess.GitHubAPIAccess.GetFiles(base.GitHubId, base.GitHubSecret,
              owner, repo, path);

        //Assert
        Assert.IsNotNull(files);
        Assert.IsTrue(files.Length > 0);
        Assert.AreEqual("autoupdate.yml", files[0].name);
        Assert.AreEqual("workflow.yml", files[1].name);
    }

}