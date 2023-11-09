namespace RepoGovernance.Core.Models.NuGetPackages
{
    public class Framework
    {
        public string framework { get; set; }
        public List<Package> topLevelPackages { get; set; }
    }
}
