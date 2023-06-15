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
                string url = $"/api/DORASummary/GetDORASummaryItem?owner={owner}&project=&repo={repo}";
                return await BaseApi.GetResponse<DORASummaryItem>(Client, url);
            }
            catch (Exception ex)
            {
                return new DORASummaryItem()
                {
                    Owner = owner,
                    Repo = repo,
                    DeploymentFrequency = 0,
                    DeploymentFrequencyBadgeURL = "https://img.shields.io/badge/Deployment%20frequency-None-lightgrey",
                    LeadTimeForChanges = 0,
                    LeadTimeForChangesBadgeURL = "https://img.shields.io/badge/Lead%20time%20for%20changes-None-lightgrey",
                    MeanTimeToRestore = 0,
                    MeanTimeToRestoreBadgeURL = "https://img.shields.io/badge/Meant%20time%20to%20restore-None-lightgrey",
                    ChangeFailureRate = -1, //This is -1 because the value is a percentage, and 0 means something
                    ChangeFailureRateBadgeURL = "https://img.shields.io/badge/Change%20failure%20rate-None-lightgrey",
                    LastUpdated = DateTime.Now,
                    LastUpdatedMessage = "Error getting DORA summary item: " + ex.ToString()
                };
            }
        }
    }
}
