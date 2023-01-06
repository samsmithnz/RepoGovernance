using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public class DevOpsMetricServiceApi
    {
        private HttpClient Client;

        public DevOpsMetricServiceApi(string devOpsServiceURL)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(devOpsServiceURL)
            };
        }

        public async Task<DORASummaryItem?> GetDORASummaryItems(string owner, string repository)
        {
            // api/DORASummary/GetDORASummaryItems?owner=samsmithnz&repository=DevOpsMetrics
            string url = $"/api/DORASummary/GetDORASummaryItems?owner={owner}&repository={repository}";
            return await BaseApi.GetResponse<DORASummaryItem>(Client, url);
        }

        
    }
}
