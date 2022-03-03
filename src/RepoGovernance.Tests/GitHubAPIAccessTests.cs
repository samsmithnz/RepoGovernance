using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
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
}