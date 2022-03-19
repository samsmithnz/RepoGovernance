namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{

    public static List<string> GetRepos(string owner)
    {
        //TODO: move this into a database, and filter by owner
        List<string> results = new()
        {
            "AzurePipelinesToGitHubActionsConverter",
            "AzurePipelinesToGitHubActionsConverterWeb",
            "Battle",
            "CustomQueue",
            "Dependabot-Configuration-Builder",
            "DevOpsMetrics",
            "DSPTree",
            "FictionBook",
            "GitHubActionsDotNet",
            "OpinionatedSoftwareAdvice",
            "RepoAutomation",
            "RepoGovernance",
            "ResearchTree",
            "Sams2048",
            "SamsFeatureFlags",
            "samsmithnz",
            "FictionBook",
            "TBS"
        };

        return results;
    }

}