using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class SummaryItemDetails
    {
        public SummaryItem? SummaryItem { get; set; }
        public bool IsContributor { get; set; }

        public SummaryItemDetails()
        {
            IsContributor = false;
        }
    }
}