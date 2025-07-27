using RepoAutomation.Core.APIAccess;
using System.Threading.Tasks;

namespace RepoGovernance.Core.Models
{
    public class SecurityAlertsResult
    {
        public int CodeScanningCount { get; set; }
        public int SecretScanningCount { get; set; }
        public int TotalCount { get; set; }

        public SecurityAlertsResult(int codeScanningCount, int secretScanningCount, int totalCount)
        {
            CodeScanningCount = codeScanningCount;
            SecretScanningCount = secretScanningCount;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Gets security alerts count from GitHub API and returns a SecurityAlertsResult object
        /// </summary>
        /// <param name="clientId">GitHub client ID</param>
        /// <param name="clientSecret">GitHub client secret</param>
        /// <param name="owner">Repository owner</param>
        /// <param name="repo">Repository name</param>
        /// <param name="state">Alert state (e.g., "open")</param>
        /// <returns>SecurityAlertsResult containing the security alerts counts</returns>
        public static async Task<SecurityAlertsResult> GetFromGitHubAsync(string? clientId, string? clientSecret, string owner, string repo, string state)
        {
            var tupleResult = await GitHubApiAccess.GetSecurityAlertsCount(clientId, clientSecret, owner, repo, state);
            return new SecurityAlertsResult(tupleResult.codeScanningCount, tupleResult.secretScanningCount, tupleResult.totalCount);
        }
    }
}