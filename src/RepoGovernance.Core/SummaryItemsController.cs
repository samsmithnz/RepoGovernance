﻿using GitHubActionsDotNet.Models.Dependabot;
using GitHubActionsDotNet.Serialization;
using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Helpers;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core
{
    public static class SummaryItemsController
    {
        public static async Task<List<SummaryItem>> GetSummaryItems(
            string? clientId,
            string? secret,
            string owner)
        {
            List<string> repos = DatabaseAccess.GetRepos(owner);

            List<SummaryItem> results = new();
            foreach (string repo in repos)
            {
                SummaryItem summaryItem = new(repo);

                //Get any actions
                List<string>? actions = await GitHubFiles.SearchForFiles(
                    clientId, secret,
                    owner, repo,
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
                List<string>? dependabot = await GitHubFiles.SearchForFiles(
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
                    summaryItem.DependabotRoot = DependabotSerialization.Deserialize(summaryItem.DependabotFile.content);
                    if (summaryItem.DependabotRoot?.updates.Count == 0)
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
                if (summaryItem.DependabotRoot?.updates != null)
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
                summaryItem.BranchPolicies = await GitHubAPIAccess.GetBranchProtectionPolicy(clientId, secret, owner, repo, "main");
                if (summaryItem.BranchPolicies == null)
                {
                    summaryItem.BranchPoliciesRecommendations.Add("Consider adding a branch policy to protect the main branch");
                }

                //Get Gitversion files
                List<string>? gitversion = await GitHubFiles.SearchForFiles(
                    clientId, secret,
                    owner, repo,
                    "GitVersion.yml", null, "");
                if (gitversion != null)
                {
                    summaryItem.GitVersion = gitversion;
                }
                else
                {
                    summaryItem.GitVersionRecommendations.Add("Consider adding Git Versioning to this repo");
                }
                //Get Frameworks
                results.Add(summaryItem);
            }

            return results;
        }
    }
}
