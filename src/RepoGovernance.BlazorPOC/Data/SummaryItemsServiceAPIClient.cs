using RepoGovernance.Core.Models;

namespace RepoGovernance.BlazorPOC.Data
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




    }
}