namespace RepoGovernance.Core.Models
{
    public class CoverallsCodeCoverage
    {
        public string? branch { get; set; }
        public string? repo_name { get; set; }
        public string? badge_url { get; set; }
        public double coverage_change { get; set; }
        public double covered_percent { get; set; }
    }
}
