using System.Text.Json;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public static class GitHubRepositoryRulesApi
    {
        /// <summary>
        /// Gets repository rulesets for a repository
        /// </summary>
        /// <param name="clientId">GitHub client ID</param>
        /// <param name="clientSecret">GitHub client secret</param>
        /// <param name="owner">Repository owner</param>
        /// <param name="repo">Repository name</param>
        /// <returns>List of repository rulesets</returns>
        public static async Task<List<RepositoryRuleset>?> GetRepositoryRulesets(string? clientId, string? clientSecret, string owner, string repo)
        {
            try
            {
                // Return empty list if credentials are missing
                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                {
                    return new List<RepositoryRuleset>();
                }

                string url = $"https://api.github.com/repos/{owner}/{repo}/rulesets?targets=branch";
                
                using (HttpClient client = new HttpClient())
                {
                    string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
                    client.DefaultRequestHeaders.Add("User-Agent", "RepoGovernance");
                    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
                    client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

                    List<RepositoryRuleset>? result = await BaseApi.GetResponse<List<RepositoryRuleset>>(client, url, true);
                    return result ?? new List<RepositoryRuleset>();
                }
            }
            catch (Exception)
            {
                // Return empty list if API call fails (e.g., repository doesn't support rulesets, not found, etc.)
                return new List<RepositoryRuleset>();
            }
        }

        /// <summary>
        /// Checks if a repository has any active branch protection via Repository Rules
        /// </summary>
        /// <param name="clientId">GitHub client ID</param>
        /// <param name="clientSecret">GitHub client secret</param>
        /// <param name="owner">Repository owner</param>
        /// <param name="repo">Repository name</param>
        /// <param name="branchName">Branch name to check (default: "main")</param>
        /// <returns>True if the branch has protection via repository rules</returns>
        public static async Task<bool> HasRepositoryRulesProtection(string? clientId, string? clientSecret, string owner, string repo, string branchName = "main")
        {
            List<RepositoryRuleset>? rulesets = await GetRepositoryRulesets(clientId, clientSecret, owner, repo);
            
            if (rulesets == null || rulesets.Count == 0)
            {
                return false;
            }

            // Check if any active rulesets apply to the specified branch and contain protection rules
            foreach (RepositoryRuleset ruleset in rulesets)
            {
                // Skip disabled rulesets
                if (ruleset.Enforcement == "disabled")
                {
                    continue;
                }

                // Check if this ruleset applies to our target branch
                if (DoesRulesetApplyToBranch(ruleset, branchName))
                {
                    // Check if it has any protection rules
                    if (HasProtectionRules(ruleset))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a ruleset applies to the specified branch
        /// </summary>
        private static bool DoesRulesetApplyToBranch(RepositoryRuleset ruleset, string branchName)
        {
            // If no conditions are specified, the ruleset applies to all branches
            if (ruleset.Conditions?.RefName == null)
            {
                return true;
            }

            // Check include patterns
            if (ruleset.Conditions.RefName.Include.Count > 0)
            {
                bool matchesInclude = false;
                foreach (string pattern in ruleset.Conditions.RefName.Include)
                {
                    if (MatchesBranchPattern(pattern, branchName))
                    {
                        matchesInclude = true;
                        break;
                    }
                }
                if (!matchesInclude)
                {
                    return false;
                }
            }

            // Check exclude patterns
            foreach (string pattern in ruleset.Conditions.RefName.Exclude)
            {
                if (MatchesBranchPattern(pattern, branchName))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Simple pattern matching for branch names (supports basic wildcard matching)
        /// </summary>
        private static bool MatchesBranchPattern(string pattern, string branchName)
        {
            // Handle exact matches
            if (pattern == branchName)
            {
                return true;
            }

            // Handle simple wildcard patterns (e.g., "refs/heads/main", "main")
            if (pattern.StartsWith("refs/heads/"))
            {
                string patternBranch = pattern.Substring("refs/heads/".Length);
                return patternBranch == branchName;
            }

            // Handle patterns like "~DEFAULT_BRANCH" (refers to the default branch)
            if (pattern == "~DEFAULT_BRANCH" && branchName == "main")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a ruleset contains protection rules
        /// </summary>
        private static bool HasProtectionRules(RepositoryRuleset ruleset)
        {
            if (ruleset.Rules == null || ruleset.Rules.Count == 0)
            {
                return false;
            }

            // These rule types indicate branch protection
            string[] protectionRuleTypes = {
                "required_status_checks",
                "required_signatures",
                "pull_request",
                "required_deployments",
                "required_conversation_resolution",
                "dismiss_stale_reviews",
                "restrict_pushes"
            };

            foreach (RepositoryRule rule in ruleset.Rules)
            {
                if (protectionRuleTypes.Contains(rule.Type))
                {
                    return true;
                }
            }

            return false;
        }
    }
}