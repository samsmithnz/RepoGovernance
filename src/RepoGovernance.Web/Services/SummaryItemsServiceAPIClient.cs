using RepoGovernance.Core.Models;

namespace RepoGovernance.Web.Services
{
    public class SummaryItemsServiceAPIClient : BaseServiceAPIClient
    {
        private readonly IConfiguration _configuration;

        public SummaryItemsServiceAPIClient(IConfiguration configuration)
        {
            _configuration = configuration;
            HttpClient client = new()
            {
                BaseAddress = new(_configuration["AppSettings:WebServiceURL"])
            };
            base.SetupClient(client);
        }

        public async Task<List<SummaryItem>> GetSummaryItems(string user)
        {
            Uri url = new($"api/SummaryItems/GetSummaryItems?user=" + user, UriKind.Relative);
            List<SummaryItem> results = await base.ReadMessageList<SummaryItem>(url);
            if (results == null)
            {
                return new List<SummaryItem>();
            }
            else
            {
                return results;
            }
        }

        public async Task<int> UpdateSummaryItem(string user, string owner, string repo)
        {
            Uri url = new($"api/SummaryItems/UpdateSummaryItem?user=" + user + "&owner=" + owner + "&repo=" + repo, UriKind.Relative);
            return await base.ReadMessageItem<int>(url);
        }

    }
}