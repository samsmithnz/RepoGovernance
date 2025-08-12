using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class RepoDetails
    {
        public string Owner { get; set; }
        public string Repository { get; set; }
        public List<TaskItem> Recommendations { get; set; }
        public List<TaskItem> IgnoredRecommendations { get; set; }
        public bool IsContributor { get; set; }
        public int TotalRecommendations => Recommendations.Count + IgnoredRecommendations.Count;
        public int ActiveRecommendations => Recommendations.Count;
        public int IgnoredCount => IgnoredRecommendations.Count;

        public RepoDetails()
        {
            Owner = "";
            Repository = "";
            Recommendations = new List<TaskItem>();
            IgnoredRecommendations = new List<TaskItem>();
            IsContributor = false;
        }

        public RepoDetails(string owner, string repository)
        {
            Owner = owner;
            Repository = repository;
            Recommendations = new List<TaskItem>();
            IgnoredRecommendations = new List<TaskItem>();
            IsContributor = false;
        }
    }
}