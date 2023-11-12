namespace RepoGovernance.Core.Models.NuGetPackages
{
    public class Project
    {
        public string path { get; set; }
        public List<Framework> frameworks { get; set; }
    }
}
