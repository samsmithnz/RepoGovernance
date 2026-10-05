using Newtonsoft.Json;

namespace RepoGovernance.Core.Models
{
    public class CoverallsCodeCoverage
    {
        public string? branch { get; set; }
        public string? repo_name { get; set; }
        public string? badge_url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double coverage_change { get; set; } = 0.0;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double covered_percent { get; set; } = 0.0;
        public string coverage_change_string
        {
            get
            {
                if (coverage_change < 0)
                {
                    return "-" + coverage_change.ToString("0");
                }
                else
                {
                    return "+" + coverage_change.ToString("0");
                }
            }
        }
    }
}
