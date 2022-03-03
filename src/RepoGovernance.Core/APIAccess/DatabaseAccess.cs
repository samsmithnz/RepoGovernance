using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{

    public static List<SummaryItem> GetSummaryItems(string owner)
    {
        //TODO: move this into a database, and filter by owner
        List<SummaryItem> results = new List<SummaryItem>()
        {
            new SummaryItem()
            {
                Repo = "AzurePipelinesToActions"
            },
            new SummaryItem()
            {
                Repo = "CustomQueue"
            },
            new SummaryItem()
            {
                Repo = "DevOpsMetrics"
            },
            new SummaryItem()
            {
                Repo = "RepoAutomation"
            },
            new SummaryItem()
            {
                Repo = "samsmithnz"
            }
        };
        return results;
    }

}