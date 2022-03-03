using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Helpers;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core
{
    public static class SummaryItemsController
    {
        public static async Task<List<SummaryItem>> GetSummaryItems(string clientId,
            string secret,
            string owner)
        {
            List<string> repos = DatabaseAccess.GetRepos(owner);

            List<SummaryItem> results = new();
            foreach (string repo in repos)
            {
                SummaryItem summaryItem = new(repo);
                List<string>? actions = await GitHubFileSearch.SearchForFiles(
                    clientId, secret,
                    owner, repo,
                    null, ".yml", ".github/workflows");
                if (actions != null)
                {
                    summaryItem.Actions = actions;
                }
            }

            return results;
        }
    }
}
