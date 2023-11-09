namespace RepoGovernance.Core.Models.NuGetPackages
{
    public class Package
    {
        public string id { get; set; }
        public string requestedVersion { get; set; }
        public string resolvedVersion { get; set; }
        public string latestVersion { get; set; }
    }
}
