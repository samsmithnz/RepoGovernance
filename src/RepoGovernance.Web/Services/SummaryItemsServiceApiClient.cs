using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Web.Services
{
    public class SummaryItemsServiceApiClient : BaseServiceApiClient, ISummaryItemsServiceApiClient
    {
        private readonly IConfiguration _configuration;

        public SummaryItemsServiceApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
            HttpClient client = new()
            {
                BaseAddress = new(_configuration["AppSettings:WebServiceURL"])
            };
            base.SetupClient(client);
        }

        public virtual async Task<List<SummaryItem>> GetSummaryItems(string user)
        {
            Uri url = new($"api/SummaryItems/GetSummaryItems?user={Uri.EscapeDataString(user)}", UriKind.Relative);
            List<SummaryItem>? results = await base.ReadMessageList<SummaryItem>(url);
            if (results == null)
            {
                return new List<SummaryItem>();
            }
            else
            {
                return results;
            }
        }

        public virtual async Task<SummaryItem?> GetSummaryItem(string user, string owner, string repo)
        {
            List<SummaryItem>? results = await GetSummaryItems(user);
            foreach (SummaryItem? item in results)
            {
                if (item.Owner == owner && item.Repo == repo)
                {
                    return item;
                }
            }
            return null;
            //Uri url = new($"api/SummaryItems/GetSummaryItem?owner=" + owner + "&repo=" + repo, UriKind.Relative);
            //SummaryItem? result = await base.ReadMessageItem<SummaryItem>(url);
            //if (result == null)
            //{
            //    throw new Exception(url.ToString());
            //    //return new SummaryItem(owner, owner, repo);
            //}
            //else
            //{
            //    return result;
            //}
        }

        public async Task<int> UpdateSummaryItem(string user, string owner, string repo)
        {
            Uri url = new($"api/SummaryItems/UpdateSummaryItem?user={Uri.EscapeDataString(user)}&owner={Uri.EscapeDataString(owner)}&repo={Uri.EscapeDataString(repo)}", UriKind.Relative);
            return await base.ReadMessageItem<int>(url);
        }

        public async Task<bool> ApproveSummaryItemPRs(//string user, 
            string owner, string repo, string approver)
        {
            Uri url = new($"api/SummaryItems/ApproveSummaryItemPRs?&owner={Uri.EscapeDataString(owner)}&repo={Uri.EscapeDataString(repo)}&approver={Uri.EscapeDataString(approver)}", UriKind.Relative);
            return await base.ReadMessageItem<bool>(url);
        }

    }
}