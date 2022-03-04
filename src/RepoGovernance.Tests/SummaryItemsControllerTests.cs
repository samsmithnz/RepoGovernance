using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using RepoGovernance.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoGovernance.Tests;

[TestClass]
public class SummaryItemsControllerTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task GetSummaryItemsTest()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        List<SummaryItem> summaryItems = await SummaryItemsController.GetSummaryItems(base.GitHubId, base.GitHubSecret, owner);

        //Assert
        Assert.IsNotNull(summaryItems);
        Assert.IsTrue(summaryItems.Count > 0);
        Assert.AreEqual("AzurePipelinesToGitHubActionsConverter", summaryItems[0].Repo);
        Assert.IsNotNull(summaryItems[0].Actions);
        Assert.AreEqual(2, summaryItems[0].Actions.Count);
        Assert.AreEqual("CustomQueue", summaryItems[1].Repo);
        Assert.IsNotNull(summaryItems[1].Actions);
        Assert.AreEqual(0, summaryItems[1].Actions.Count);
    }

}