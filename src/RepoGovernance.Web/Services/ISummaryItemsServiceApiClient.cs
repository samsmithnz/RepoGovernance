using RepoGovernance.Core.Models;

namespace RepoGovernance.Web.Services
{
    public interface ISummaryItemsServiceApiClient
    {
        Task<List<SummaryItem>> GetSummaryItems(string user);
        Task<SummaryItem?> GetSummaryItem(string user, string owner, string repo);
        Task<int> UpdateSummaryItem(string user, string owner, string repo);
        Task<bool> ApproveSummaryItemPRs(string owner, string repo, string approver);
    }
}