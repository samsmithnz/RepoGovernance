namespace RepoGovernance.Core.Models
{
    public class SummaryItem
    {
        public SummaryItem(string repo)
        {
            Repo = repo;
            Actions = new();
            Dependabot = new();
            BranchPolicies = new();
            GitVersion = new();
            Frameworks = new();
        }

        public string Repo { get; internal set; }
        public List<string> Actions { get; set; }
        public List<string> Dependabot { get; set; }
        public List<string> BranchPolicies { get; set; }
        public List<string> GitVersion { get; set; }
        public List<string> Frameworks { get; set; }
    }
}
