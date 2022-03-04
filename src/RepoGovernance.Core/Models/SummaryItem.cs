namespace RepoGovernance.Core.Models
{
    public class SummaryItem
    {
        public SummaryItem(string repo)
        {
            Repo = repo;
            Actions = new();
            Dependabot = new();
        }

        public string Repo { get; internal set; }
        public List<string> Actions { get; set; }
        public List<string> Dependabot { get; set; }
    }
}
