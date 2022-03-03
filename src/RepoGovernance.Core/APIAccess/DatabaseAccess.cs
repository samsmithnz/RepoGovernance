using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{

    public static List<SummaryItem> GetSummaryItems()
    {
        //TODO: move this into a databvase
        List<SummaryItem> results = new List<SummaryItem>()
        {
            new SummaryItem()
            {
                Repo = "AzurePipelinesToActions"
            },
            new SummaryItem()
            {
                Repo = "DevOpsMetrics"
            },
            new SummaryItem()
            {
                Repo = "RepoAutomation"
            }
        };
        return results;
    }

}