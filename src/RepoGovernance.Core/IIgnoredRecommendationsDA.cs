using RepoGovernance.Core.Models;

namespace RepoGovernance.Core
{
    public interface IIgnoredRecommendationsDA
    {
        Task<List<IgnoredRecommendation>> GetIgnoredRecommendations(string user);
        Task<List<IgnoredRecommendation>> GetIgnoredRecommendations(string user, string owner, string repository);
        Task<bool> IgnoreRecommendation(string user, string owner, string repository, string recommendationType, string recommendationDetails);
        Task<bool> UnignoreRecommendation(string user, string owner, string repository, string recommendationType, string recommendationDetails);
        Task<bool> IsRecommendationIgnored(string user, string owner, string repository, string recommendationType, string recommendationDetails);
    }
}