﻿using DotNetCensus.Core.Models;
using GitHubActionsDotNet.Serialization;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Helpers;
using RepoAutomation.Core.Models;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Helpers;
using RepoGovernance.Core.Models;
using RepoGovernance.Core.Models.NuGetPackages;
using RepoGovernance.Core.TableStorage;

namespace RepoGovernance.Core
{
    public static class SummaryItemsDA
    {
        public static async Task<List<UserOwnerRepo>> GetRepos(string? connectionString, string user)
        {
            //return DatabaseAccess.GetRepos(user);

            List<UserOwnerRepo> results;
            if (connectionString != null)
            {
                results = await AzureTableStorageDA.GetUserOwnerRepoItemsFromTable(connectionString, "UserOwnerRepo", user);
            }
            else
            {
                throw new ArgumentException("connectionstring is null");
            }
            //sort the list
            results = results.OrderBy(o => o.Repo).ToList();
            return results;
        }

        /// <summary>
        /// Get a list of summary items from Azure Storage
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<List<SummaryItem>> GetSummaryItems(
            string? connectionString,
            string owner)
        {
            List<SummaryItem> results;
            if (connectionString != null)
            {
                results = await AzureTableStorageDA.GetSummaryItemsFromTable(connectionString, "Summary", owner);
            }
            else
            {
                throw new ArgumentException("connectionstring is null");
            }
            //sort the list
            results = results.OrderBy(o => o.Repo).ToList();
            return results;
        }

        public static async Task<SummaryItem?> GetSummaryItem(string connectionString,
            string user, string owner, string repo)
        {
            SummaryItem? result = null;
            List<SummaryItem> summaryItems = await GetSummaryItems(connectionString, user);
            if (summaryItems != null)
            {
                foreach (SummaryItem item in summaryItems)
                {
                    if (item.Owner == owner && item.Repo == repo)
                    {
                        result = item;
                        break;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Update a single repo's record into Azure Storage
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <param name="connectionString"></param>
        /// <param name="owner"></param>
        /// <param name="repo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<int> UpdateSummaryItem(string? clientId,
            string? secret,
            string? connectionString,
            string? devOpsServiceURL,
            string user,
            string owner,
            string repo,
            string? azureTenantId,
            string? azureClientId,
            string? azureClientSecret,
            AzureDeployment? azureDeployment = null)
        {
            int itemsUpdated = 0;
            //Initialize the summary item
            SummaryItem? summaryItem = new(user, owner, repo);

            if (azureDeployment == null && connectionString != null)
            {
                //check to make sure there isn't data in the table already for Azure deployments, and pull it out to save it
                SummaryItem? existingItem = await GetSummaryItem(connectionString, user, owner, repo);
                if (existingItem != null && existingItem.AzureDeployment != null)
                {
                    azureDeployment = existingItem.AzureDeployment;
                }
                //save the nuget packages - since they are updated in a separate process, and we don't want to lose them here.
                if (existingItem != null && existingItem.NuGetPackages.Count > 0)
                {
                    summaryItem.NuGetPackages = existingItem.NuGetPackages;
                }
            }

            try
            {
                //Get repo settings
                RepoAutomation.Core.Models.Repo? repoSettings = await GitHubApiAccess.GetRepo(clientId, secret, owner, repo);
                if (repoSettings != null)
                {
                    summaryItem.RepoSettings = repoSettings;
                    if (summaryItem.RepoSettings.allow_auto_merge == false)
                    {
                        summaryItem.RepoSettingsRecommendations.Add("Consider enabling 'Allow Auto-Merge' in repo settings to streamline PR merging");
                    }
                    if (summaryItem.RepoSettings.delete_branch_on_merge == false)
                    {
                        summaryItem.RepoSettingsRecommendations.Add("Consider disabling 'Delete branch on merge' in repo settings to streamline PR merging and auto-cleanup completed branches");
                    }
                    if (summaryItem.RepoSettings.allow_rebase_merge == true)
                    {
                        summaryItem.RepoSettingsRecommendations.Add("Consider disabling 'Allow rebase merge' in repo settings, as rebasing can be confusing");
                    }
                }

                //Get any actions
                List<string>? actions = await GitHubFiles.GetFiles(clientId, secret, owner, repo,
                null, null, ".github/workflows");
                if (actions != null)
                {
                    summaryItem.Actions = actions;
                }
                if (summaryItem.Actions.Count == 0)
                {
                    summaryItem.ActionRecommendations.Add("Consider adding an action to build your project");
                }

                //Get any dependabot files
                List<string>? dependabot = await GitHubFiles.GetFiles(
                    clientId, secret,
                    owner, repo,
                    "dependabot.yml", null, ".github");
                if (dependabot != null)
                {
                    summaryItem.Dependabot = dependabot;
                }
                if (summaryItem.Dependabot.Count >= 1)
                {
                    if (summaryItem.Dependabot.Count > 1)
                    {
                        summaryItem.DependabotRecommendations.Add("Consider consolidating your Dependabot files to just one file");
                    }
                    summaryItem.DependabotFile = await GitHubFiles.GetFileContents(clientId, secret, owner, repo, ".github/dependabot.yml");
                    summaryItem.DependabotRoot = DependabotSerialization.Deserialize(summaryItem?.DependabotFile?.content);
                    if (summaryItem?.DependabotRoot?.updates.Count == 0)
                    {
                        summaryItem.DependabotRecommendations.Add("Dependabot file exists, but is not configured to scan any manifest files");
                    }
                }
                else
                {
                    summaryItem.DependabotRecommendations.Add("Consider adding a Dependabot file to automatically update dependencies");
                }
                //Check each line of the dependabot file
                int actionsCount = 0;
                if (summaryItem?.DependabotRoot?.updates != null)
                {
                    foreach (GitHubActionsDotNet.Models.Dependabot.Package? item in summaryItem.DependabotRoot.updates)
                    {
                        if (item.package_ecosystem == "github-actions")
                        {
                            actionsCount++;
                        }
                        if (item.assignees == null || item.assignees.Count == 0)
                        {
                            summaryItem.DependabotRecommendations.Add("Consider adding an assignee to ensure the Dependabot PR has an owner to the " + item.directory + " project, " + item.package_ecosystem + " ecosystem");
                        }
                        if (item.open_pull_requests_limit == null)
                        {
                            summaryItem.DependabotRecommendations.Add("Consider adding an open_pull_requests_limit to ensure Dependabot doesn't open too many PR's in the " + item.directory + " project, " + item.package_ecosystem + " ecosystem");
                        }
                        if (item.groups == null)
                        {
                            summaryItem.DependabotRecommendations.Add("Consider adding groups to consolidate Dependabot PR's in the " + item.directory + " project, " + item.package_ecosystem + " ecosystem");
                        }
                    }
                    if (summaryItem.Actions.Count > 0 && actionsCount == 0)
                    {
                        summaryItem.DependabotRecommendations.Add("Consider adding github-actions ecosystem to Dependabot to auto-update actions dependencies");
                    }
                }

                //Get branch policies
                BranchProtectionPolicy? branchPolicies = await GitHubApiAccess.GetBranchProtectionPolicy(clientId, secret, owner, repo, "main");
                if (branchPolicies == null)
                {
                    summaryItem?.BranchPoliciesRecommendations.Add("Consider adding a branch policy to protect the main branch");
                }
                else if (summaryItem != null)
                {
                    summaryItem.BranchPolicies = branchPolicies;
                    if (summaryItem.BranchPolicies.enforce_admins == null || summaryItem.BranchPolicies.enforce_admins.enabled == false)
                    {
                        summaryItem.BranchPoliciesRecommendations.Add("Consider enabling 'Enforce Admins', to ensure that all users of the repo must follow branch policy rules");
                    }
                    if (summaryItem.BranchPolicies.required_conversation_resolution == null || summaryItem.BranchPolicies.required_conversation_resolution.enabled == false)
                    {
                        summaryItem.BranchPoliciesRecommendations.Add("Consider enabling 'Require Conversation Resolution', to ensure that all comments have been resolved in the PR before merging to the main branch");
                    }
                    if (summaryItem?.BranchPolicies?.required_status_checks?.checks == null || summaryItem.BranchPolicies.required_status_checks.checks.Length == 0)
                    {
                        summaryItem?.BranchPoliciesRecommendations.Add("Consider adding status checks to the branch policy, to ensure that builds and tests pass successfully before the branch is merged to the main branch");
                    }
                }

                //Get Gitversion files
                List<string>? gitversion = await GitHubFiles.GetFiles(
                    clientId, secret,
                    owner, repo,
                    "GitVersion.yml", null, "");
                if (summaryItem != null && gitversion != null && gitversion.Count > 0)
                {
                    summaryItem.GitVersion = gitversion;
                }
                else
                {
                    summaryItem?.GitVersionRecommendations.Add("Consider adding Git Versioning to this repo");
                }

                //Get Frameworks, using the DotNetCensus library we built in another project
                DotNetCensus.Core.Models.Repo? repo2 = new(owner, repo)
                {
                    User = clientId,
                    Password = secret,
                    Branch = "main"
                };
                List<FrameworkSummary> frameworkSummaries = DotNetCensus.Core.Main.GetFrameworkSummary(null, repo2, false);
                foreach (FrameworkSummary frameworkSummary in frameworkSummaries)
                {
                    Models.Framework framework = new()
                    {
                        Name = frameworkSummary.Framework,
                        Color = DotNetRepoScanner.GetColorFromStatus(frameworkSummary.Status),
                        Count = frameworkSummary.Count
                    };
                    if (frameworkSummary.Framework != null &&
                        summaryItem?.DotNetFrameworks.Where(p => p.Name == frameworkSummary.Framework).FirstOrDefault() == null)
                    {
                        summaryItem?.DotNetFrameworks.Add(framework);
                    }
                }
                //Order the frameworks so they appear in alphabetically
                if (summaryItem != null && summaryItem.DotNetFrameworks != null)
                {
                    summaryItem.DotNetFrameworks = summaryItem.DotNetFrameworks.OrderBy(o => o.Name).ToList();
                }

                //Get the last commit
                string? lastCommitSha = await GitHubApiAccess.GetLastCommit(clientId, secret, owner, repo);
                if (summaryItem != null && lastCommitSha != null)
                {
                    summaryItem.LastCommitSha = lastCommitSha;
                }

                //Get Pull Request details
                List<PullRequest> pullRequests = await GitHubApiAccess.GetPullRequests(clientId, secret, owner, repo);
                if (summaryItem != null && pullRequests != null && pullRequests.Count > 0)
                {
                    foreach (PullRequest pr in pullRequests)
                    {
                        //Check if the PR has been reviewed
                        if (pr.Number != null)
                        {
                            List<PRReview> prReviews = await GitHubApiAccess.GetPullRequestReview(clientId, secret, owner, repo, pr.Number);
                            if (prReviews != null && prReviews.Count > 0 && prReviews[^1] != null)
                            {
                                string? state = prReviews[^1].state;
                                if (state == "APPROVED")
                                {
                                    pr.Approved = true;
                                }
                            }
                        }
                        //Check if the PR has was created by dependabot
                        foreach (string prLabel in pr.Labels)
                        {
                            if (prLabel == "dependencies")
                            {
                                pr.IsDependabotPR = true;
                            }
                        }
                    }
                    summaryItem.PullRequests = pullRequests;
                }

                //Get DevOps Metrics
                if (summaryItem != null && devOpsServiceURL != null)
                {
                    DevOpsMetricServiceApi devopsAPI = new(devOpsServiceURL);
                    DORASummaryItem? dORASummaryItem = await devopsAPI.GetDORASummaryItems(owner, repo);
                    if (dORASummaryItem != null)
                    {
                        summaryItem.DORASummary = dORASummaryItem;
                    }
                }

                //Get the Release info
                Release? release = await GitHubApiAccess.GetReleaseLatest(clientId, secret, owner, repo);
                if (summaryItem != null && release != null)
                {
                    summaryItem.Release = release;
                }

                //Get Coveralls.io Code Coverage
                CoverallsCodeCoverage? coverallsCodeCoverage = await CoverallsCodeCoverageApi.GetCoverallsCodeCoverage(owner, repo);
                if (summaryItem != null && coverallsCodeCoverage != null)
                {
                    summaryItem.CoverallsCodeCoverage = coverallsCodeCoverage;
                }

                //Get SonarCloud metrics (note that we use user here instead of owner)
                SonarCloud? sonarCloud = await SonarCloudApi.GetSonarCloudMetrics(user, repo);
                if (summaryItem != null && sonarCloud != null)
                {
                    summaryItem.SonarCloud = sonarCloud;
                }

                //Get Repo Language stats
                List<RepoLanguage>? repoLanguages = await RepoLanguageHelper.GetRepoLanguages(clientId, secret, owner, repo);
                if (summaryItem != null && repoLanguages != null && repoLanguages.Count > 0)
                {
                    summaryItem.RepoLanguages = repoLanguages;
                    summaryItem.RepoLanguagesLastUpdated = DateTime.Now;
                }
                else if (summaryItem != null && repoLanguages == null && connectionString != null)
                {
                    //If we couldn't find languages last time - lets pull up the existing summary item and use that
                    SummaryItem? summaryItem2 = await GetSummaryItem(connectionString, user, owner, repo);
                    if (summaryItem2 != null && summaryItem2.RepoLanguages != null)
                    {
                        summaryItem.RepoLanguages = summaryItem2.RepoLanguages;
                        if (summaryItem2.RepoLanguagesLastUpdated != null)
                        {
                            summaryItem.RepoLanguagesLastUpdated = summaryItem2.RepoLanguagesLastUpdated;
                        }
                    }
                }

                //If there are azure deployment records, then process the summary item and save the detail
                if (azureDeployment != null &&
                    azureTenantId != null &&
                    azureClientId != null &&
                    azureClientSecret != null)
                {
                    AzureApi azureApi = new(azureTenantId, azureClientId, azureClientSecret);
                    azureDeployment = await azureApi.GetApplications(azureDeployment);
                    if (summaryItem != null && azureDeployment != null)
                    {
                        summaryItem.AzureDeployment = azureDeployment;
                    }
                }

            }
            catch (Exception ex)
            {
                //If there was an error, update with the error message
                string message = ex.ToString();
                if (connectionString != null)
                {
                    summaryItem = await GetSummaryItem(connectionString, user, owner, repo);
                    if (summaryItem != null)
                    {
                        summaryItem.LastUpdatedMessage = "Error at " + DateTime.Now.ToString() + ": " + ex.ToString();
                    }
                }
            }

            //Save the summary item
            if (connectionString != null && summaryItem != null)
            {
                itemsUpdated += await AzureTableStorageDA.UpdateSummaryItemsIntoTable(connectionString, user, owner, repo, summaryItem);
            }
            else
            {
                throw new ArgumentException("connectionstring is null");
            }

            return itemsUpdated;
        }

        //TODO: convert this bool to int to count updates!
        public static async Task<bool> ApproveSummaryItemPRs(string? clientId,
            string? secret,
            //string? connectionString,
            //string? devOpsServiceURL,
            //string user,
            string owner,
            string repo,
            string approver)
        {
            //int itemsUpdated = 0;

            bool result = await GitHubApiAccess.ApprovePullRequests(clientId, secret, owner, repo, approver);

            return result;
            ;
        }

        public static async Task<int> UpdateSummaryItemNuGetPackageStats(
            string? connectionString,
            string user,
            string owner,
            string repo,
            string nugetPayload,
            string payloadType)
        {
            int itemsUpdated = 0;

            //Get the summary item
            SummaryItem? summaryItem = await GetSummaryItem(connectionString, user, owner, repo);
            if (summaryItem != null && nugetPayload != null && payloadType != null)
            {
                //Process the NuGet Package JSON payload
                DotNetPackages dotNetPackages = new();
                List<NugetPackage>? nugetResults = null;
                switch (payloadType)
                {
                    case "Deprecated":
                        nugetResults = dotNetPackages.GetNugetPackagesDeprecated(nugetPayload);
                        break;
                    case "Outdated":
                        nugetResults = dotNetPackages.GetNugetPackagesOutdated(nugetPayload);
                        break;
                    case "Vulnerable":
                        nugetResults = dotNetPackages.GetNugetPackagesVulnerable(nugetPayload);
                        break;
                }
                if (nugetResults != null)
                {
                    //First remove results with the same type
                    summaryItem.NuGetPackages.RemoveAll(x => x.Type == payloadType);

                    //Add the results to the summary item
                    summaryItem.NuGetPackages.AddRange(nugetResults);
                    itemsUpdated += await AzureTableStorageDA.UpdateSummaryItemsIntoTable(connectionString, user, owner, repo, summaryItem);
                }
            }

            return itemsUpdated;
        }
    }
}
