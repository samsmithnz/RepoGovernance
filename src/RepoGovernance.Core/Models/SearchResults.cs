using RepoAutomation.Core.Models;

namespace RepoGovernance.Core.Models
{
    public class SearchResults
    {
        public int total_count { get; set; }
        public List<Repo> items { get; set; }
    }
}
