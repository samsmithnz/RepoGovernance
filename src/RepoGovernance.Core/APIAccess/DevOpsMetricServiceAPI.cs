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
            return await GetResponse<DORASummaryItem>(Client, url);
        }

        private static async Task<T?> GetResponse<T>(HttpClient client, string url)
        {
            T obj = default;
            if (client != null && url != null)
            {
                Debug.WriteLine("Running url: " + client.BaseAddress.ToString() + url);
                Console.WriteLine("Running url: " + client.BaseAddress.ToString() + url);
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode == true)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseBody) == false)
                        {
                            obj = JsonConvert.DeserializeObject<T>(responseBody);
                        }
                    }
                    else
                    {
                        //Throw an exception
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            return obj;
        }
    }
}
