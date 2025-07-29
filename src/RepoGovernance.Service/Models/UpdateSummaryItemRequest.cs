namespace RepoGovernance.Service.Models
{
    public class UpdateSummaryItemRequest
    {
        public string? User { get; set; }
        public string? Owner { get; set; }
        public string? Repo { get; set; }
        public string? NugetDeprecatedPayload { get; set; }
        public string? NugetOutdatedPayload { get; set; }
        public string? NugetVulnerablePayload { get; set; }
    }
}