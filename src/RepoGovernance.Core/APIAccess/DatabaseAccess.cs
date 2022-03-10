namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{

    public static List<string> GetRepos(string owner)
    {
        //TODO: move this into a database, and filter by owner
        List<string> results = new()
        {
            "AzurePipelinesToGitHubActionsConverter",
            "CustomQueue",
            "DevOpsMetrics",
            "RepoAutomation",
            "samsmithnz"
        };

        return results;
    }

    }