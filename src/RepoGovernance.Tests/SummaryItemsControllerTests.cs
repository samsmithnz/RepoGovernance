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
    public async Task UpdateSummaryItemsTest()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        int itemsUpdated = await SummaryItemsController.UpdateSummaryItems(GitHubId, GitHubSecret, AzureStorageConnectionString, owner, 1);

        //Assert
        Assert.AreEqual(1, itemsUpdated);
    }

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
        Assert.IsNotNull(item1.RepoSettings);
        Assert.AreEqual(0, item1.RepoSettingsRecommendations.Count);
        Assert.AreEqual(2, item1.Actions.Count);
        Assert.AreEqual(0, item1.ActionRecommendations.Count);
        Assert.AreEqual(1, item1.Dependabot.Count);
        Assert.AreEqual("dependabot.yml", item1.DependabotFile?.name);
        Assert.AreEqual("2", item1.DependabotRoot?.version);
        Assert.IsTrue(item1.DependabotFile?.content?.Length > 0);
        Assert.AreEqual(1, item1.DependabotRecommendations.Count);
        Assert.AreEqual("Consider adding an open_pull_requests_limit to ensure Dependabot doesn't open too many PR's in the / project, github-actions ecosystem", item1.DependabotRecommendations[0]);
        Assert.IsNotNull(item1.BranchPolicies);
        Assert.AreEqual(0, item1.BranchPoliciesRecommendations.Count);
        Assert.AreEqual(1, item1.GitVersion.Count);
        Assert.AreEqual(0, item1.GitVersionRecommendations.Count);
        Assert.AreEqual(2, item1.DotNetFrameworks.Count);
        Assert.AreEqual("netstandard2.0", item1.DotNetFrameworks[0]);
        Assert.AreEqual("net6.0", item1.DotNetFrameworks[1]);
        Assert.AreEqual(0, item1.DotNetFrameworksRecommendations.Count);

        //second repo
        SummaryItem item2 = summaryItems[1];
        Assert.AreEqual("CustomQueue", item2.Repo);
        Assert.IsNotNull(item2.RepoSettings);
        Assert.AreEqual(3, item2.RepoSettingsRecommendations.Count);
        Assert.AreEqual("Consider enabling 'Allow Auto-Merge' in repo settings to streamline PR merging", item2.RepoSettingsRecommendations[0]);
        Assert.AreEqual("Consider disabling 'Delete branch on merge' in repo settings to streamline PR merging and auto-cleanup completed branches", item2.RepoSettingsRecommendations[1]);
        Assert.AreEqual("Consider disabling 'Allow rebase merge' in repo settings, as rebasing is confusing and dumb", item2.RepoSettingsRecommendations[2]);
        Assert.AreEqual(0, item2.Actions.Count);
        Assert.AreEqual(1, item2.ActionRecommendations.Count);
        Assert.AreEqual("Consider adding an action to build your project", item2.ActionRecommendations[0]);
        Assert.AreEqual(0, item2.Dependabot.Count);
        Assert.AreEqual(null, item2.DependabotFile);
        Assert.AreEqual(null, item2.DependabotRoot);
        Assert.AreEqual(1, item2.DependabotRecommendations.Count);
        Assert.AreEqual("Consider adding a Dependabot file to automatically update dependencies", item2.DependabotRecommendations[0]);
        Assert.IsNull(item2.BranchPolicies);
        Assert.AreEqual(1, item2.BranchPoliciesRecommendations.Count);
        Assert.AreEqual("Consider adding a branch policy to protect the main branch", item2.BranchPoliciesRecommendations[0]);
        Assert.AreEqual(0, item2.GitVersion.Count);
        Assert.AreEqual(1, item2.GitVersionRecommendations.Count);
        Assert.AreEqual("Consider adding Git Versioning to this repo", item2.GitVersionRecommendations[0]);
        Assert.AreEqual(1, item2.DotNetFrameworks.Count);
        Assert.AreEqual("net6.0", item2.DotNetFrameworks[0]);
        Assert.AreEqual(0, item2.DotNetFrameworksRecommendations.Count);
    }

}