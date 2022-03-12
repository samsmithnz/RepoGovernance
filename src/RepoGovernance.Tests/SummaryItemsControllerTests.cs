using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
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
        List<SummaryItem> summaryItems = await SummaryItemsController.GetSummaryItems(GitHubId, GitHubSecret, owner);

        //Assert
        Assert.IsNotNull(summaryItems);
        Assert.IsTrue(summaryItems.Count > 0);

        //first repo
        SummaryItem item1 = summaryItems[0];
        Assert.AreEqual("AzurePipelinesToGitHubActionsConverter", item1.Repo);
        Assert.AreEqual(2, item1.Actions.Count);
        Assert.AreEqual(1, item1.Dependabot.Count);
        Assert.AreEqual("dependabot.yml", item1.DependabotFile.name);
        Assert.IsTrue(item1.DependabotFile?.content?.Length > 0);
        Assert.IsNotNull(item1.BranchPolicies);
        Assert.AreEqual(1, item1.GitVersion.Count);
        Assert.AreEqual(0, item1.Frameworks.Count);

        //second repo
        SummaryItem item2 = summaryItems[1];
        Assert.AreEqual("CustomQueue", item2.Repo);
        Assert.AreEqual(0, item2.Actions.Count);
        Assert.AreEqual(0, item2.Dependabot.Count);
        Assert.AreEqual(null, item2.DependabotFile);
        Assert.IsNull(item2.BranchPolicies);
        Assert.AreEqual(0, item2.GitVersion.Count);
        Assert.AreEqual(0, item2.Frameworks.Count);
    }

}