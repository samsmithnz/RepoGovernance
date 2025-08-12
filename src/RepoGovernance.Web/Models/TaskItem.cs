using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class TaskItem
    {
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string RecommendationType { get; set; }
        public string RecommendationDetails { get; set; }
        public string Id { get; set; }

        public TaskItem(string owner, string repository, string recommendationType, string recommendationDetails)
        {
            Owner = owner;
            Repository = repository;
            RecommendationType = recommendationType;
            RecommendationDetails = recommendationDetails;
            Id = $"{owner}|{repository}|{recommendationType}|{recommendationDetails}";
        }
    }
}