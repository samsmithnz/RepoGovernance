using RepoGovernance.Core.Models;

namespace RepoGovernance.Web.Services
{
    public interface ISummaryItemsServiceApiClient
    {
        Task<List<SummaryItem>> GetSummaryItems(string user);
        Task<SummaryItem?> GetSummaryItem(string user, string owner, string repo);
        Task<int> UpdateSummaryItem(string user, string owner, string repo);
        Task<bool> ApproveSummaryItemPRs(string owner, string repo, string approver);
        Task<bool> IgnoreRecommendation(string user, string owner, string repo, string recommendationType, string recommendationDetails);
        Task<bool> RestoreRecommendation(string user, string owner, string repo, string recommendationType, string recommendationDetails);
        Task<List<IgnoredRecommendation>> GetIgnoredRecommendations(string user, string owner, string repo);
        Task<List<IgnoredRecommendation>> GetAllIgnoredRecommendations(string user);
    }
}