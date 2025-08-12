using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Core.Models
{
    [ExcludeFromCodeCoverage]
    public class IgnoredRecommendation
    {
        public string User { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string RecommendationType { get; set; }
        public string RecommendationDetails { get; set; }
        public DateTime IgnoredDate { get; set; }

        public IgnoredRecommendation(string user, string owner, string repository, string recommendationType, string recommendationDetails)
        {
            User = user;
            Owner = owner;
            Repository = repository;
            RecommendationType = recommendationType;
            RecommendationDetails = recommendationDetails;
            IgnoredDate = DateTime.UtcNow;
        }

        // Parameterless constructor needed for deserialization
        public IgnoredRecommendation()
        {
            User = "";
            Owner = "";
            Repository = "";
            RecommendationType = "";
            RecommendationDetails = "";
            IgnoredDate = DateTime.UtcNow;
        }

        public string GetUniqueId()
        {
            return $"{Owner}|{Repository}|{RecommendationType}|{RecommendationDetails}";
        }
    }
}