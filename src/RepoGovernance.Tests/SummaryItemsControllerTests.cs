using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;
using RepoGovernance.Tests.Helpers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace RepoGovernance.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class SummaryItemsControllerTests : BaseAPIAccessTests
{
    [TestMethod]
    public async Task UpdateRepoGovernanceSummaryItemTest()
    {
        //Arrange
        string user = "samsmithnz";
        string owner = "samsmithnz";
        string repo = "RepoGovernance";
        AzureDeployment azureDeployment = new()
        {
            DeployedURL = "https://repogovernance-prod-eu-web.azurewebsites.net/",
            AppRegistrations = new()
            {
                new AzureAppRegistration() { Name = "RepoGovernancePrincipal2023" },
                new AzureAppRegistration() { Name = "RepoGovernanceGraphAPIAccess" }
            }
        };

        //Act - runs a repo in about 4s
        int itemsUpdated = await SummaryItemsDA.UpdateSummaryItem(
            GitHubId, 
            GitHubSecret, 
            AzureStorageConnectionString, 
            DevOpsServiceURL, 
            user, owner, repo,
            AzureTenantId,
            AzureClientId,
            AzureClientSecret,
            azureDeployment);

        //Assert
        Assert.AreEqual(1, itemsUpdated);
    }

    [TestMethod]
    public async Task UpdateDevOpsMetricsSummaryItemTest()
    {
        //Arrange
        string user = "samsmithnz";
        string owner = "DeveloperMetrics";
        string repo = "DevOpsMetrics";
        AzureDeployment azureDeployment = null;
        //AzureDeployment azureDeployment = new()
        //{
        //    DeployedURL = "https://devops-prod-eu-web.azurewebsites.net//",
        //    AppRegistrations = new()
        //    {
        //        new AzureAppRegistration() { Name = "DeveloperMetricsOrgSP2023" },
        //        new AzureAppRegistration() { Name = "DevOpsMetrics" },
        //        new AzureAppRegistration() { Name = "DevOpsMetricsServicePrincipal2022" }
        //    }
        //};

        //Act - runs a repo in about 4s
        int itemsUpdated = await SummaryItemsDA.UpdateSummaryItem(
            GitHubId, 
            GitHubSecret, 
            AzureStorageConnectionString, 
            DevOpsServiceURL, 
            user, owner, repo,
            AzureTenantId,
            AzureClientId,
            AzureClientSecret,
            azureDeployment);

        //Assert
        Assert.AreEqual(1, itemsUpdated);
    }

    [TestMethod]
    public async Task UpdateSamsFeatureFlagsSummaryItemTest()
    {
        //Arrange
        string user = "samsmithnz";
        string owner = "samsmithnz";
        string repo = "SamsFeatureFlags";
        AzureDeployment azureDeployment = new()
        {
            DeployedURL = "https://featureflags-prod-eu-web.azurewebsites.net/",
            AppRegistrations = new()
            {
                new AzureAppRegistration() { Name = "SamsFeatureFlagsAzureSP" },
                new AzureAppRegistration() { Name = "SamsFeatureFlagsSP2022" }
            }
        };

        //Act - runs a repo in about 4s
        int itemsUpdated = await SummaryItemsDA.UpdateSummaryItem(
            GitHubId, 
            GitHubSecret, 
            AzureStorageConnectionString, 
            DevOpsServiceURL, 
            user, owner, repo,
            AzureTenantId,
            AzureClientId,
            AzureClientSecret,
            azureDeployment);

        //Assert
        Assert.AreEqual(1, itemsUpdated);
    }

    [TestMethod]
    public async Task UpdateSamSmithNZdotComSummaryItemTest()
    {
        //Arrange
        string user = "samsmithnz";
        string owner = "SamSmithNZ-dotcom";
        string repo = "SamSmithNZ.com";
        AzureDeployment azureDeployment = new()
        {
            DeployedURL = "https://samsmithnz.com",
            AppRegistrations = new()
            {
                new AzureAppRegistration() { Name = "SamsSmithDotComServicePrincipal2022" }
            }
        };

        //Act - runs a repo in about 4s
        int itemsUpdated = await SummaryItemsDA.UpdateSummaryItem(
            GitHubId, 
            GitHubSecret, 
            AzureStorageConnectionString, 
            DevOpsServiceURL, 
            user, owner, repo,
            AzureTenantId,
            AzureClientId,
            AzureClientSecret,
            azureDeployment);

        //Assert
        Assert.AreEqual(1, itemsUpdated);
    }

    [TestMethod]
    public async Task UpdateAzurePipelinesToGitHubActionsConverterWebSummaryItemTest()
    {
        //Arrange
        string user = "samsmithnz";
        string owner = "samsmithnz";
        string repo = "AzurePipelinesToGitHubActionsConverterWeb";
        AzureDeployment azureDeployment = new()
        {
            DeployedURL = "https://pipelinestoactions.azurewebsites.net/",
            AppRegistrations = new()
            {
                new AzureAppRegistration() { Name = "SamsPipelinesToActions2022ServicePrincipal" }
            }
        };

        //Act - runs a repo in about 4s
        int itemsUpdated = await SummaryItemsDA.UpdateSummaryItem(
            GitHubId, 
            GitHubSecret, 
            AzureStorageConnectionString, 
            DevOpsServiceURL, 
            user, owner, repo,
            AzureTenantId,
            AzureClientId,
            AzureClientSecret,
            azureDeployment);

        //Assert
        Assert.AreEqual(1, itemsUpdated);
    }

    //[TestMethod]
    //public async Task UpdateAllItemsTest()
    //{
    //    //Arrange
    //    string user = "samsmithnz";
    //    //string owner = "samsmithnz";
    //    string serviceUrl = "https://devops-prod-eu-service.azurewebsites.net";

    //    //Act - runs each repo in about 4s
    //    List<UserOwnerRepo> repos = SummaryItemsDA.GetRepos(user);

    //    int itemsUpdated = 0;
    //    foreach (UserOwnerRepo repo in repos)
    //    {
    //        string ownerName = repo.Owner;
    //        string repoName = repo.Repo;
    //        itemsUpdated += await SummaryItemsDA.UpdateSummaryItem(GitHubId, GitHubSecret,
    //            AzureStorageConnectionString,
    //            serviceUrl,
    //            user, ownerName, repoName);
    //    }

    //    //Assert
    //    Assert.AreEqual(repos.Count, itemsUpdated);
    //}

    [TestMethod]
    public async Task GetSummaryItemsTest()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        List<SummaryItem> summaryItems = await SummaryItemsDA.GetSummaryItems(AzureStorageConnectionString, owner);

        //Assert
        Assert.IsNotNull(summaryItems);
        Assert.IsTrue(summaryItems.Count > 0);

        //first repo
        SummaryItem? item1 = summaryItems.Where(r => r.Repo == "AzurePipelinesToGitHubActionsConverter").FirstOrDefault();
        Assert.IsNotNull(item1);
        if (item1 != null)
        {
            Assert.AreEqual("AzurePipelinesToGitHubActionsConverter", item1.Repo);
            Assert.IsNotNull(item1.RepoSettings);
            Assert.AreEqual(0, item1.RepoSettingsRecommendations.Count);
            Assert.AreEqual(2, item1.Actions.Count);
            Assert.AreEqual(0, item1.ActionRecommendations.Count);
            Assert.AreEqual(1, item1.Dependabot.Count);
            Assert.AreEqual("dependabot.yml", item1.DependabotFile?.name);
            Assert.AreEqual("2", item1.DependabotRoot?.version);
            Assert.IsTrue(item1.DependabotFile?.content?.Length > 0);
            Assert.AreEqual(0, item1.DependabotRecommendations.Count);
            Assert.AreEqual("Consider adding an open_pull_requests_limit to ensure Dependabot doesn't open too many PR's in the / project, github-actions ecosystem", item1.DependabotRecommendations[0]);
            Assert.IsNotNull(item1.BranchPolicies);
            Assert.AreEqual(0, item1.BranchPoliciesRecommendations.Count);
            Assert.AreEqual(1, item1.GitVersion.Count);
            Assert.AreEqual(0, item1.GitVersionRecommendations.Count);
            Assert.AreEqual(2, item1.DotNetFrameworks.Count);
            Assert.AreEqual(".NET 7.0", item1.DotNetFrameworks[0].Name);
            Assert.AreEqual("bg-primary", item1.DotNetFrameworks[0].Color);
            Assert.AreEqual(".NET Standard 2.0", item1.DotNetFrameworks[1].Name);
            Assert.AreEqual("bg-primary", item1.DotNetFrameworks[1].Color);
            Assert.AreEqual(0, item1.DotNetFrameworksRecommendations.Count);
            Assert.IsNotNull(item1.Release);
            Assert.IsNotNull(item1.Release?.ToTimingString());
            Assert.IsTrue(item1.PullRequests.Count >= 0);
            Assert.IsNotNull(item1.CoverallsCodeCoverage);
            Assert.IsNotNull(item1.SonarCloud);
            if (item1.SonarCloud != null)
            {
                Assert.IsNotNull(item1.SonarCloud.CodeSmellsBadgeImage);
                Assert.AreEqual("https://sonarcloud.io/project/issues?resolved=false&types=CODE_SMELL&id=samsmithnz_AzurePipelinesToGitHubActionsConverter", item1.SonarCloud.CodeSmellsLink);
                Assert.IsNotNull(item1.SonarCloud.BugsBadgeImage);
                Assert.AreEqual("https://sonarcloud.io/project/issues?resolved=false&types=BUG&id=samsmithnz_AzurePipelinesToGitHubActionsConverter", item1.SonarCloud.BugsLink);
                Assert.IsNotNull(item1.SonarCloud.LinesOfCodeBadgeImage);
                Assert.AreEqual("https://sonarcloud.io/component_measures?metric=ncloc&id=samsmithnz_AzurePipelinesToGitHubActionsConverter", item1.SonarCloud.LinesOfCodeLink);
            }
            Assert.IsNotNull(item1.RepoLanguages);
            Assert.IsTrue(item1.RepoLanguages.Count > 0);
            Assert.IsNull(item1.AzureDeployment);
        }

        //second repo
        SummaryItem? item2 = summaryItems.Where(r => r.Repo == "CustomQueue").FirstOrDefault();
        Assert.IsNotNull(item2);
        if (item2 != null)
        {
            Assert.AreEqual("CustomQueue", item2.Repo);
            Assert.IsNotNull(item2.RepoSettings);
            Assert.AreEqual(1, item2.RepoSettingsRecommendations.Count);
            Assert.AreEqual("Consider disabling 'Allow rebase merge' in repo settings, as rebasing can be confusing", item2.RepoSettingsRecommendations[0]);
            Assert.AreEqual(1, item2.Actions.Count);
            Assert.AreEqual(0, item2.ActionRecommendations.Count);
            Assert.AreEqual(0, item2.Dependabot.Count);
            Assert.AreEqual(null, item2.DependabotFile);
            Assert.AreEqual(null, item2.DependabotRoot);
            Assert.AreEqual(1, item2.DependabotRecommendations.Count);
            Assert.AreEqual("Consider adding a Dependabot file to automatically update dependencies", item2.DependabotRecommendations[0]);
            Assert.IsNotNull(item2.BranchPolicies);
            Assert.AreEqual(1, item2.BranchPoliciesRecommendations.Count);
            Assert.AreEqual("Consider enabling 'Enforce Admins', to ensure that all users of the repo must follow branch policy rules", item2.BranchPoliciesRecommendations[0]);
            Assert.AreEqual(1, item2.GitVersion.Count);
            Assert.AreEqual(0, item2.GitVersionRecommendations.Count);
            //Assert.AreEqual("Consider adding Git Versioning to this repo", item2.GitVersionRecommendations[0]);
            Assert.AreEqual(1, item2.DotNetFrameworks.Count);
            Assert.AreEqual(".NET 7.0", item2.DotNetFrameworks[0].Name);
            Assert.AreEqual("bg-primary", item2.DotNetFrameworks[0].Color);
            Assert.AreEqual(0, item2.DotNetFrameworksRecommendations.Count);
            Assert.IsNotNull(item2.Release);
            Assert.IsTrue(item2.PullRequests.Count >= 0);
            Assert.IsNull(item2.AzureDeployment);
        }

        //third repo
        SummaryItem? item3 = summaryItems.Where(r => r.Repo == "DevOpsMetrics").FirstOrDefault();
        Assert.IsNotNull(item3);
        if (item3 != null)
        {
            Assert.AreEqual("DevOpsMetrics", item3.Repo);
            //TODO: Includes 4 duplicates of .net6, should this be .net6 x4?
            Assert.IsTrue(item3.DotNetFrameworks.Count >= 0);
            Assert.AreEqual("public", item3.RepoSettings.visibility);
            Assert.IsNotNull(item3.DORASummary);
            Assert.IsNotNull(item3.SonarCloud);
            Assert.IsNotNull(item3.AzureDeployment);
        }

        //fourth repo
        SummaryItem? item4 = summaryItems.Where(r => r.Repo == "TBS").FirstOrDefault();
        Assert.IsNotNull(item4);
        if (item4 != null)
        {
            Assert.AreEqual("TBS", item4.Repo);
            //Assert.AreEqual(1, item4.DotNetFrameworks.Count);
            //Assert.AreEqual("Unity3d v2020.3", item4.DotNetFrameworks[0].Name);
            //Assert.AreEqual("bg-primary", item4.DotNetFrameworks[0].Color);
            Assert.AreEqual("private", item4.RepoSettings.visibility);
            Assert.AreEqual(0, item4.BranchPoliciesRecommendations.Count);
        }

        //Fifth repo
        SummaryItem? item5 = summaryItems.Where(r => r.Repo == "ResearchTree").FirstOrDefault();
        Assert.IsNotNull(item5);
        if (item5 != null)
        {
            Assert.IsTrue(item5.DotNetFrameworks.Count >= 4);
            Assert.AreEqual(".NET 6.0", item5.DotNetFrameworks[0].Name);
            Assert.AreEqual("bg-primary", item5.DotNetFrameworks[0].Color);
            Assert.AreEqual(".NET 6.0-windows", item5.DotNetFrameworks[1].Name);
            Assert.AreEqual("bg-primary", item5.DotNetFrameworks[1].Color);
            Assert.AreEqual(".NET Framework 4.7.1", item5.DotNetFrameworks[2].Name);
            Assert.AreEqual("bg-primary", item5.DotNetFrameworks[2].Color);
            Assert.AreEqual(".NET Standard 2.0", item5.DotNetFrameworks[3].Name);
            Assert.AreEqual("bg-primary", item5.DotNetFrameworks[3].Color);
        }

        //Sixth repo
        SummaryItem? item6 = summaryItems.Where(r => r.Repo == "RepoAutomationUnitTests").FirstOrDefault();
        Assert.IsNotNull(item6);
        if (item6 != null)
        {
            Assert.IsTrue(item6.PullRequests.Count >= 1);
            Assert.IsNotNull(item6.RepoSettings);
            Assert.IsNotNull(item6.RepoSettingsRecommendations);
            if (item6.RepoSettingsRecommendations != null)
            {
                Assert.AreEqual(3, item6.RepoSettingsRecommendations.Count);
                Assert.AreEqual("Consider enabling 'Allow Auto-Merge' in repo settings to streamline PR merging", item6.RepoSettingsRecommendations[0]);
                Assert.AreEqual("Consider disabling 'Delete branch on merge' in repo settings to streamline PR merging and auto-cleanup completed branches", item6.RepoSettingsRecommendations[1]);
                Assert.AreEqual("Consider disabling 'Allow rebase merge' in repo settings, as rebasing can be confusing", item6.RepoSettingsRecommendations[2]);
            }
        }


        //Ensure they are alphabetical
        Assert.AreEqual("TurnBasedEngine", summaryItems[^1].Repo);
    }

    [TestMethod]
    public void GetRepoItemsTest()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        List<UserOwnerRepo> repos = SummaryItemsDA.GetRepos(owner);

        //Assert
        Assert.IsNotNull(repos);
        Assert.IsTrue(repos.Count > 0);
    }
}