using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{
    public static List<ProfileOwnerRepo> GetRepos(string profile)
    {
        //TODO: move this into a database
        List<ProfileOwnerRepo> results = new()
        {
            new ProfileOwnerRepo(profile, profile, "AzurePipelinesToGitHubActionsConverter"),
            new ProfileOwnerRepo(profile, profile, "AzurePipelinesToGitHubActionsConverterWeb"),
            new ProfileOwnerRepo(profile, profile, "Battle"),
            new ProfileOwnerRepo(profile, profile, "CustomQueue"),
            new ProfileOwnerRepo(profile, profile, "Dependabot-Configuration-Builder"),
            new ProfileOwnerRepo(profile, profile, "DevOpsMetrics"),
            new ProfileOwnerRepo(profile, profile, "DSPTree"),
            new ProfileOwnerRepo(profile, profile, "FictionBook"),
            new ProfileOwnerRepo(profile, profile, "GitHubActionsDotNet"),
            new ProfileOwnerRepo(profile, profile, "OpinionatedSoftwareAdvice"),
            new ProfileOwnerRepo(profile, profile, "RepoAutomation"),
            new ProfileOwnerRepo(profile, profile, "RepoGovernance"),
            new ProfileOwnerRepo(profile, profile, "ResearchTree"),
            new ProfileOwnerRepo(profile, profile, "Sams2048"),
            new ProfileOwnerRepo(profile, profile, "SamsFeatureFlags"),
            new ProfileOwnerRepo(profile, profile, "samsmithnz"),
            new ProfileOwnerRepo(profile, profile, "TBS"),
            new ProfileOwnerRepo(profile, "SamSmithNZ-dotcom", "SamSmithNZ.com"),
            new ProfileOwnerRepo(profile, "SamSmithNZ-dotcom", "MandMCounter")
        };

        return results;
    }

}