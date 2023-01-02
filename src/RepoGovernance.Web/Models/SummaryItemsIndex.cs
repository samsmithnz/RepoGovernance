using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Web.Models
{
    public class SummaryItemsIndex
    {
        public List<SummaryItem> SummaryItems { get; set; }
        public IOrderedEnumerable<KeyValuePair<string, int>> SummaryRepoLanguages { get; set; }
        public int SummaryRepoLanguagesTotal { get; set; }
    }
}
