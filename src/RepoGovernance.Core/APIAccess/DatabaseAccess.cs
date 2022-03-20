namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{
    public static List<(string,string)> GetRepos(string owner)
    {
        List<(string, string)> results;

        //TODO: move this into a database
        if (owner == "samsmithnz")
        {
            results = new()
            {
                (owner,"AzurePipelinesToGitHubActionsConverter"),
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
                (owner, "TBS")
            };
        }
        else if (owner == "SamSmithNZ-dotcom")
        {
            results = new()
            {
                (owner, "SamSmithNZ.com"),
                (owner, "MandMCounter")
            };
        }
        else
        {
            results = new();
        }

        return results;
    }

}