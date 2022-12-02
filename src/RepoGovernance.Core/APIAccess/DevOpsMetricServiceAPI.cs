using Newtonsoft.Json;
using RepoGovernance.Core.Models;
using System.Diagnostics;

namespace RepoGovernance.Core.APIAccess
{
    public class DevOpsMetricServiceAPI
    {
        private HttpClient Client;

        public DevOpsMetricServiceAPI(string devOpsServiceURL)
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
            return await BaseAPI.GetResponse<DORASummaryItem>(Client, url);
        }

        
    }
}
