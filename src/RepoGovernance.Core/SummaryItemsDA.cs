﻿using GitHubActionsDotNet.Models.Dependabot;
using GitHubActionsDotNet.Serialization;
using Newtonsoft.Json;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Helpers;
using RepoAutomation.Core.Models;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Helpers;
using RepoGovernance.Core.Models;
using RepoGovernance.Core.TableStorage;

namespace RepoGovernance.Core
{
    public static class SummaryItemsDA
    {
        public static List<SummaryItem> GetSummaryItems(
            string? connectionString,
            string owner)
        {
            List<SummaryItem> results = AzureTableStorageDA.GetSummaryItemsFromTable(connectionString, "Summary", owner);
            return results;
        }

        public static async Task<int> UpdateSummaryItems(string? clientId,
            string? secret,
            string? connectionString,
            string owner,
            int itemsToUpdate = 0)
        {
            int itemsUpdated = 0;

            //Get the Repos we are tracking
            List<string> repos = DatabaseAccess.GetRepos(owner);

            //Collect data for each repo and save it in azure table storage
            foreach (string repo in repos)
            {
                if (itemsToUpdate != 0 && itemsToUpdate <= itemsUpdated)
                {
                    break;
                }
                SummaryItem summaryItem = new(repo);

                //Get repo settings
                Repo? repoSettings = await GitHubAPIAccess.GetRepo(clientId, secret, owner, repo);
                if (repoSettings != null)
                {
                    summaryItem.RepoSettings = repoSettings;
                }
                if (summaryItem.RepoSettings != null)
                {
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
                        summaryItem.RepoSettingsRecommendations.Add("Consider disabling 'Allow rebase merge' in repo settings, as rebasing is confusing and dumb");
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
                        summaryItem.DependabotRecommendations.Add("Consider consilidating your Dependabot files to just one file");
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
                    foreach (Package? item in summaryItem.DependabotRoot.updates)
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
                    }
                    if (summaryItem.Actions.Count > 0 && actionsCount == 0)
                    {
                        summaryItem.DependabotRecommendations.Add("Consider adding github-actions ecosystem to Dependabot to auto-update actions dependencies");
                    }
                }

                //Get branch policies
                BranchProtectionPolicy? branchPolicies = await GitHubAPIAccess.GetBranchProtectionPolicy(clientId, secret, owner, repo, "main");
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

                //Get Frameworks
                List<Project> projects = await DotNetRepoScanner.ScanRepo(clientId, secret, owner, repo);
                if (projects != null)
                {
                    foreach (Project project in projects)
                    {
                        if (project.Framework != null)
                        {
                            summaryItem?.DotNetFrameworks.Add(project.Framework);
                        }
                    }
                }

                //Save the summary item
                string json = JsonConvert.SerializeObject(summaryItem);
                itemsUpdated += await AzureTableStorageDA.UpdateSummaryItemsIntoTable(connectionString, owner, repo, json);
            }

            return itemsUpdated;
        }
    }
}