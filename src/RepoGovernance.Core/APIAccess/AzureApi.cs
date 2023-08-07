using RepoGovernance.Core.Models.Azure;

namespace RepoGovernance.Core.APIAccess
{

    public class AzureApi : BaseApi
    {
        public async Task<List<Application>> GetApplications()
        {
            string url = "https://graph.microsoft.com/v1.0/applications";
            return await BaseApi.GetResponse<Application>(Client, url);

        }
    }
}
