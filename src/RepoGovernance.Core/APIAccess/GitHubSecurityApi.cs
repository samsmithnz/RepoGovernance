using System.Text;

namespace RepoGovernance.Core.APIAccess
{
    public static class GitHubSecurityApi
    {
        public static async Task<int> GetSecurityAlertsCount(string? clientId, string? secret, string owner, string repo)
        {
            int alertsCount = 0;
            
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(secret))
            {
                return alertsCount;
            }

            try
            {
                using HttpClient client = new();
                
                // Set up GitHub API authentication
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{secret}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.Add("User-Agent", "RepoGovernance");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

                // Try to get code scanning alerts first
                string codeScanningUrl = $"https://api.github.com/repos/{owner}/{repo}/code-scanning/alerts?state=open";
                string? codeScanningResponse = await GetResponseString(client, codeScanningUrl, true);
                
                if (!string.IsNullOrEmpty(codeScanningResponse) && codeScanningResponse != "[]")
                {
                    // Parse the JSON array to count alerts
                    var codeScanningAlerts = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(codeScanningResponse);
                    if (codeScanningAlerts != null)
                    {
                        alertsCount += codeScanningAlerts.Length;
                    }
                }

                // Try to get secret scanning alerts
                string secretScanningUrl = $"https://api.github.com/repos/{owner}/{repo}/secret-scanning/alerts?state=open";
                string? secretScanningResponse = await GetResponseString(client, secretScanningUrl, true);
                
                if (!string.IsNullOrEmpty(secretScanningResponse) && secretScanningResponse != "[]")
                {
                    // Parse the JSON array to count alerts
                    var secretScanningAlerts = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(secretScanningResponse);
                    if (secretScanningAlerts != null)
                    {
                        alertsCount += secretScanningAlerts.Length;
                    }
                }
            }
            catch (Exception)
            {
                // If we can't access security alerts (permissions, private repo, etc.), return 0
                alertsCount = 0;
            }

            return alertsCount;
        }

        private static async Task<string?> GetResponseString(HttpClient client, string url, bool ignoreErrors = false)
        {
            try
            {
                using HttpResponseMessage response = await client.GetAsync(url);
                
                if (ignoreErrors || response.IsSuccessStatusCode)
                {
                    if (response.StatusCode.ToString() != "NotFound")
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception)
            {
                // Ignore errors if ignoreErrors is true
                if (!ignoreErrors)
                {
                    throw;
                }
            }
            
            return null;
        }
    }
}