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
        //first repo
        Assert.AreEqual("AzurePipelinesToGitHubActionsConverter", summaryItems[0].Repo);
        Assert.AreEqual(2, summaryItems[0].Actions.Count);
        Assert.AreEqual(1, summaryItems[0].Dependabot.Count);
        Assert.AreEqual(0, summaryItems[0].BranchPolicies.Count);
        Assert.AreEqual(1, summaryItems[0].GitVersion.Count);
        Assert.AreEqual(0, summaryItems[0].Frameworks.Count);
        //second repo
        Assert.AreEqual("CustomQueue", summaryItems[1].Repo);
        Assert.AreEqual(0, summaryItems[1].Actions.Count);
        Assert.AreEqual(0, summaryItems[1].Dependabot.Count);
        Assert.AreEqual(0, summaryItems[1].BranchPolicies.Count);
        Assert.AreEqual(0, summaryItems[1].GitVersion.Count);
        Assert.AreEqual(0, summaryItems[1].Frameworks.Count);
    }

}