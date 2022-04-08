using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;
using RepoGovernance.Tests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepoGovernance.Tests;

[TestClass]
public class SummaryItemsControllerTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task UpdateRepoGovernanceSummaryItemsTest()
    {
        //Arrange
        string user = "samsmithnz";
        string owner = "samsmithnz";
        string repo = "TBS";

        //Act - runs each repo in about 4s
        int itemsUpdated = await SummaryItemsDA.UpdateSummaryItems(GitHubId, GitHubSecret, AzureStorageConnectionString, user, owner, repo);

        //Assert
        Assert.AreEqual(1, itemsUpdated);
    }

    //[TestMethod]
    //public async Task UpdateAllItemsTest()
    //{
    //    //Arrange
    //    string user = "samsmithnz";
    //    string owner = "samsmithnz";

    //    //Act - runs each repo in about 4s
    //    List<UserOwnerRepo> repos = SummaryItemsDA.GetRepos(user);

    //    int itemsUpdated = 0;
    //    foreach (UserOwnerRepo repo in repos)
    //    {
    //        string ownerName = repo.Owner;
    //        string repoName = repo.Repo;
    //        itemsUpdated += await SummaryItemsDA.UpdateSummaryItems(GitHubId, GitHubSecret, AzureStorageConnectionString, user, ownerName, repoName);
    //    }

    //    //Assert
    //    Assert.AreEqual(repos.Count, itemsUpdated);
    //}

    [TestMethod]
    public void GetSummaryItemsTest()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        List<SummaryItem> summaryItems = SummaryItemsDA.GetSummaryItems(AzureStorageConnectionString, owner);

        //Assert
        Assert.IsNotNull(summaryItems);
        Assert.IsTrue(summaryItems.Count > 0);

        //first repo
        SummaryItem? item1 = summaryItems.Where(r => r.Repo == "AzurePipelinesToGitHubActionsConverter").FirstOrDefault();
        Assert.IsNotNull(item1);
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
        Assert.AreEqual("netstandard2.0", item1.DotNetFrameworks[0].Name);
        Assert.AreEqual("bg-primary", item1.DotNetFrameworks[0].Color);
        Assert.AreEqual("net6.0", item1.DotNetFrameworks[1].Name);
        Assert.AreEqual("bg-primary", item1.DotNetFrameworks[1].Color);
        Assert.AreEqual(0, item1.DotNetFrameworksRecommendations.Count);

        //second repo
        SummaryItem? item2 = summaryItems.Where(r => r.Repo == "CustomQueue").FirstOrDefault();
        Assert.IsNotNull(item2);
        Assert.AreEqual("CustomQueue", item2.Repo);
        Assert.IsNotNull(item2.RepoSettings);
        Assert.AreEqual(3, item2.RepoSettingsRecommendations.Count);
        Assert.AreEqual("Consider enabling 'Allow Auto-Merge' in repo settings to streamline PR merging", item2.RepoSettingsRecommendations[0]);
        Assert.AreEqual("Consider disabling 'Delete branch on merge' in repo settings to streamline PR merging and auto-cleanup completed branches", item2.RepoSettingsRecommendations[1]);
        Assert.AreEqual("Consider disabling 'Allow rebase merge' in repo settings, as rebasing can be confusing", item2.RepoSettingsRecommendations[2]);
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
        Assert.AreEqual("net6.0", item2.DotNetFrameworks[0].Name);
        Assert.AreEqual("bg-primary", item2.DotNetFrameworks[0].Color);
        Assert.AreEqual(0, item2.DotNetFrameworksRecommendations.Count);

        //third repo
        SummaryItem? item3 = summaryItems.Where(r => r.Repo == "DevOpsMetrics").FirstOrDefault();
        Assert.IsNotNull(item3);
        Assert.AreEqual("DevOpsMetrics", item3.Repo);
        //TODO: Includes 4 duplicates of .net6, should this be .net6 x4?
        Assert.AreEqual(3, item3.DotNetFrameworks.Count);
        Assert.AreEqual("public", item3.RepoSettings.visibility);

        //fifth repo
        SummaryItem? item4 = summaryItems.Where(r => r.Repo == "TBS").FirstOrDefault();
        Assert.IsNotNull(item4);
        Assert.AreEqual("TBS", item4.Repo);
        //Assert.AreEqual(1, item4.DotNetFrameworks.Count);
        //Assert.AreEqual("Unity3d v2020.3", item4.DotNetFrameworks[0].Name);
        //Assert.AreEqual("bg-primary", item4.DotNetFrameworks[0].Color);
        Assert.AreEqual("private", item4.RepoSettings.visibility);
        Assert.AreEqual(0, item4.BranchPoliciesRecommendations.Count);

        SummaryItem? item5 = summaryItems.Where(r => r.Repo == "ResearchTree").FirstOrDefault();
        Assert.IsNotNull(item5);
        Assert.AreEqual(4, item5.DotNetFrameworks.Count);
        //Assert.AreEqual("net6.0-windows", item5.DotNetFrameworks[^2].Name);
        //Assert.AreEqual("bg-primary", item5.DotNetFrameworks[^2].Color);
        Assert.AreEqual(".NET Framework v4.7.1", item5.DotNetFrameworks[^1].Name);
        Assert.AreEqual("bg-warning", item5.DotNetFrameworks[^1].Color);

        //Ensure they are alphabetical
        Assert.AreEqual("TBS", summaryItems[^1].Repo);
    }

}