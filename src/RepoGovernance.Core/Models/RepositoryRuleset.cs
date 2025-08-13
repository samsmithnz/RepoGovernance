using System.Text.Json.Serialization;

namespace RepoGovernance.Core.Models
{
    public class RepositoryRuleset
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("target")]
        public string Target { get; set; } = string.Empty;

        [JsonPropertyName("source_type")]
        public string SourceType { get; set; } = string.Empty;

        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;

        [JsonPropertyName("enforcement")]
        public string Enforcement { get; set; } = string.Empty;

        [JsonPropertyName("conditions")]
        public RepositoryRulesetConditions? Conditions { get; set; }

        [JsonPropertyName("rules")]
        public List<RepositoryRule> Rules { get; set; } = new List<RepositoryRule>();
    }

    public class RepositoryRulesetConditions
    {
        [JsonPropertyName("ref_name")]
        public RefNameCondition? RefName { get; set; }
    }

    public class RefNameCondition
    {
        [JsonPropertyName("include")]
        public List<string> Include { get; set; } = new List<string>();

        [JsonPropertyName("exclude")]
        public List<string> Exclude { get; set; } = new List<string>();
    }

    public class RepositoryRule
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("parameters")]
        public object? Parameters { get; set; }
    }
}