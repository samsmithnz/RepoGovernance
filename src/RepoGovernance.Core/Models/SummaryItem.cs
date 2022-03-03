namespace RepoGovernance.Core.Models
{
    public class SummaryItem
    {
        public SummaryItem()
        {
            Actions = new List<string>();
        }

        public string Repo { get; set; }
        public List<string> Actions { get; set; }
    }
}
