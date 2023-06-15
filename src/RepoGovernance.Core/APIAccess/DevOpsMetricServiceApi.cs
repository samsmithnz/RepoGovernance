using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public class DevOpsMetricServiceApi
    {
        private readonly HttpClient Client;

        public DevOpsMetricServiceApi(string devOpsServiceURL)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(devOpsServiceURL)
            };
        }

        public async Task<DORASummaryItem?> GetDORASummaryItems(string owner, string repo)
        {
            try
            {
                // api/DORASummary/GetDORASummaryItems?owner=samsmithnz&repository=DevOpsMetrics
                string url = $"/api/DORASummary/GetDORASummaryItem?owner={owner}&repository={repo}";
                return await BaseApi.GetResponse<DORASummaryItem>(Client, url);
            }
            catch (Exception ex)
            {
                return new DORASummaryItem()
                {
                    Owner = owner,
                    Repo = repo,
                    ChangeFailureRate = -1,
                    LastUpdated = DateTime.Now,
                    LastUpdatedMessage = "Error getting DORA summary item: " + ex.ToString()
                };
            }
        }
    }
}
