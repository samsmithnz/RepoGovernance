using GitHubActionsDotNet.Models.Dependabot;
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
                    null, null, ".github/workflows"); //"*.yml"
                if (actions != null)
                {
                    summaryItem.Actions = actions;
                }
                //Get any dependabot files
                List<string>? dependabot = await GitHubFiles.SearchForFiles(
                    clientId, secret,
                    owner, repo,
                    "dependabot.yml", null, ".github"); //"*.yml"
                if (dependabot != null)
                {
                    summaryItem.Dependabot = dependabot;
                    if (dependabot.Count > 0)
                    {
                        summaryItem.DependabotFile = await GitHubFiles.GetFileContents(clientId, secret, owner, repo, ".github/dependabot.yml");
                        DependabotRoot dependabotRoot = DependabotSerialization.Deserialize(summaryItem.DependabotFile.content);
                    }
                }

                //Get branch policies
                summaryItem.BranchPolicies = await GitHubAPIAccess.GetBranchProtectionPolicy(clientId, secret, owner, repo, "main");

                //Get Gitversion files
                List<string>? gitversion = await GitHubFiles.SearchForFiles(
                    clientId, secret,
                    owner, repo,
                    "GitVersion.yml", null, null); //"*.yml"
                if (gitversion != null)
                {
                    summaryItem.GitVersion = gitversion;
                }
                //Get Frameworks
                results.Add(summaryItem);
            }

            return results;
        }
    }
}
