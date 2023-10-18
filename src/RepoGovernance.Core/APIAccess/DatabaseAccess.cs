using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess;

public static class DatabaseAccess
{
    public static List<UserOwnerRepo> GetRepos(string user)
    {
        //TODO: move this into a database
        List<UserOwnerRepo> results = new()
        {
            new UserOwnerRepo(user, user, "AzurePipelinesToGitHubActionsConverter"),
            new UserOwnerRepo(user, user, "AzurePipelinesToGitHubActionsConverterWeb"),
            new UserOwnerRepo(user, user, "CustomQueue"),
            new UserOwnerRepo(user, user, "Dependabot-Configuration-Builder"),
            new UserOwnerRepo(user, user, "DotNetCensus"),
            new UserOwnerRepo(user, user, "DSPTree"),
            new UserOwnerRepo(user, user, "FactorySim"),
            new UserOwnerRepo(user, user, "FictionBook"),
            new UserOwnerRepo(user, user, "GitHubActionsDotNet"),
            new UserOwnerRepo(user, user, "OpinionatedSoftwareAdvice"),
            new UserOwnerRepo(user, user, "MermaidDotNet"),
            new UserOwnerRepo(user, user, "PuzzleSolver"),
            new UserOwnerRepo(user, user, "RepoAutomation"),
            new UserOwnerRepo(user, user, "RepoAutomationUnitTests"),
            new UserOwnerRepo(user, user, "RepoGovernance"),
            new UserOwnerRepo(user, user, "ResearchTree"),
            new UserOwnerRepo(user, user, "Sams2048"),
            new UserOwnerRepo(user, user, "SamsFeatureFlags"),
            new UserOwnerRepo(user, user, "samsmithnz"),
            new UserOwnerRepo(user, user, "SatisfactoryTree"),
            new UserOwnerRepo(user, user, "SamsDotNetSonarCloudAction"),            
            new UserOwnerRepo(user, user, "TBS"),
            new UserOwnerRepo(user, user, "TurnBasedEngine"),
            new UserOwnerRepo(user, "SamSmithNZ-dotcom", "SamSmithNZ.com"),
            new UserOwnerRepo(user, "SamSmithNZ-dotcom", "MandMCounter"),
            new UserOwnerRepo(user, "DeveloperMetrics", "DevOpsMetrics"),
            new UserOwnerRepo(user, "DeveloperMetrics", "deployment-frequency"),
            new UserOwnerRepo(user, "DeveloperMetrics", "lead-time-for-changes"),
        };

        return results;
    }

}