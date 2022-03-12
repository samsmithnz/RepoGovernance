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
        Assert.AreEqual(0, item1.ActionRecommendations.Count);
        Assert.AreEqual(1, item1.Dependabot.Count);
        Assert.AreEqual("dependabot.yml", item1.DependabotFile.name);
        Assert.AreEqual("2", item1.DependabotRoot.version);
        Assert.IsTrue(item1.DependabotFile?.content?.Length > 0);
        Assert.AreEqual(1, item1.DependabotRecommendations.Count);
        Assert.AreEqual("Consider adding an open_pull_requests_limit to ensure Dependabot doesn't open too many PR's in the / project, github-actions ecosystem", item1.DependabotRecommendations[0]);
        Assert.IsNotNull(item1.BranchPolicies);
        Assert.AreEqual(1, item1.GitVersion.Count);
        Assert.AreEqual(0, item1.Frameworks.Count);

        //second repo
        SummaryItem item2 = summaryItems[1];
        Assert.AreEqual("CustomQueue", item2.Repo);
        Assert.AreEqual(0, item2.Actions.Count);
        Assert.AreEqual(0, item2.ActionRecommendations.Count);
        Assert.AreEqual(0, item2.Dependabot.Count);
        Assert.AreEqual(null, item2.DependabotFile);
        Assert.AreEqual(null, item2.DependabotRoot);
        Assert.AreEqual(1, item2.DependabotRecommendations.Count);
        Assert.AreEqual("Consider adding a Dependabot file to automatically update dependencies", item2.DependabotRecommendations[0]);
        //Assert.AreEqual("Consider adding an open_pull_requests_limit to ensure Dependabot doesn't open too many PR's in the / project, github-actions ecosystem", item1.DependabotRecommendations[0]);
        //Assert.AreEqual("Consider adding an open_pull_requests_limit to ensure Dependabot doesn't open too many PR's in the / project, github-actions ecosystem", item1.DependabotRecommendations[0]);
        Assert.IsNull(item2.BranchPolicies);
        Assert.AreEqual(0, item2.GitVersion.Count);
        Assert.AreEqual(0, item2.Frameworks.Count);
    }

}