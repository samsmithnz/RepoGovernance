using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using RepoGovernance.Tests.Helpers;
using System.Threading.Tasks;

namespace RepoGovernance.Tests;

[TestClass]
public class GitHubAPIAccessTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetReposTest()
    {
        //Arrange
        string owner = "samsmithnz";
        string repository = "RepoGovernance";

        //Act
        Repo? repo = await GitHubAPIAccess.GetRepo(base.GitHubId, base.GitHubSecret, 
            owner, repository);

        //Assert
        Assert.IsNotNull(repo);
    }
}