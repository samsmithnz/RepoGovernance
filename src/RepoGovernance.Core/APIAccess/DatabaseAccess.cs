namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{
    public static List<(string, string)> GetRepos(string owner)
    {
        //TODO: move this into a database
        List<(string, string)> results = new()
        {
            (owner, "AzurePipelinesToGitHubActionsConverter"),
            (owner, "AzurePipelinesToGitHubActionsConverterWeb"),
            (owner, "Battle"),
            (owner, "CustomQueue"),
            (owner, "Dependabot-Configuration-Builder"),
            (owner, "DevOpsMetrics"),
            (owner, "DSPTree"),
            (owner, "FictionBook"),
            (owner, "GitHubActionsDotNet"),
            (owner, "OpinionatedSoftwareAdvice"),
            (owner, "RepoAutomation"),
            (owner, "RepoGovernance"),
            (owner, "ResearchTree"),
            (owner, "Sams2048"),
            (owner, "SamsFeatureFlags"),
            (owner, "samsmithnz"),
            (owner, "TBS"),
            ("SamSmithNZ-dotcom", "SamSmithNZ.com"),
            ("SamSmithNZ-dotcom", "MandMCounter")
        };

        return results;
    }

}