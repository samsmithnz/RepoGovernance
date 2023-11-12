namespace RepoGovernance.Core.Models.NuGetPackages
{
    public class Package
    {
        public string id { get; set; }
        public string requestedVersion { get; set; }
        public string resolvedVersion { get; set; }
        public string latestVersion { get; set; }
        public string[] deprecationReasons { get; set; }
        public List<Vulnerability> vulnerabilities { get; set; }
        public string GetFirstVulnerability()
        {
            if (vulnerabilities != null && vulnerabilities.Count > 0)
            {
                return vulnerabilities[0].severity;
            }
            return string.Empty;
        }
    }
}
