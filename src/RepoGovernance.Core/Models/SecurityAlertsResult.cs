using System;
using System.Net.Http;
using System.Text.Json;
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
            int codeScanningCount = 0;
            int secretScanningCount = 0;

            using (HttpClient client = new HttpClient())
            {
                // Set up authentication if provided
                if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));
                }

                client.DefaultRequestHeaders.Add("User-Agent", "RepoGovernance");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");

                try
                {
                    // Get code scanning alerts
                    string codeScanningUrl = $"https://api.github.com/repos/{owner}/{repo}/code-scanning/alerts?state={state}";
                    HttpResponseMessage codeScanningResponse = await client.GetAsync(codeScanningUrl);
                    if (codeScanningResponse.IsSuccessStatusCode)
                    {
                        string codeScanningContent = await codeScanningResponse.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(codeScanningContent))
                        {
                            codeScanningCount = doc.RootElement.GetArrayLength();
                        }
                    }

                    // Get secret scanning alerts
                    string secretScanningUrl = $"https://api.github.com/repos/{owner}/{repo}/secret-scanning/alerts?state={state}";
                    HttpResponseMessage secretScanningResponse = await client.GetAsync(secretScanningUrl);
                    if (secretScanningResponse.IsSuccessStatusCode)
                    {
                        string secretScanningContent = await secretScanningResponse.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(secretScanningContent))
                        {
                            secretScanningCount = doc.RootElement.GetArrayLength();
                        }
                    }
                }
                catch
                {
                    // If there's an error accessing security alerts, return zeros
                    // This handles cases where the repository doesn't have security features enabled
                    // or access is restricted
                }
            }

            int totalCount = codeScanningCount + secretScanningCount;
            return new SecurityAlertsResult(codeScanningCount, secretScanningCount, totalCount);
        }
    }
}