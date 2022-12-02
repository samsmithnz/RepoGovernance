using GitHubActionsDotNet.Models.Dependabot;
using RepoAutomation.Core.Models;

namespace RepoGovernance.Core.Models
{
    public class SummaryItem
    {
        public SummaryItem(string user, string owner, string repo)
        {
            LastUpdated = DateTime.Now;
            User = user;
            Owner = owner;
            Repo = repo;
            RepoSettings = new();
            RepoSettingsRecommendations = new();
            Actions = new();
            ActionRecommendations = new();
            Dependabot = new();
            DependabotRecommendations = new();
            BranchPoliciesRecommendations = new();
            GitVersion = new();
            GitVersionRecommendations = new();
            DotNetFrameworks = new();
            DotNetFrameworksRecommendations = new();
            PullRequests = new();
        }

        public string User { get; internal set; }
        public string Owner { get; internal set; }
        public string Repo { get; internal set; }
        public Repo RepoSettings { get; set; }
        public List<string> RepoSettingsRecommendations { get; set; }
        public List<string> Actions { get; set; }
        public List<string> ActionRecommendations { get; set; }
        public List<string> Dependabot { get; set; }
        public GitHubFile? DependabotFile { get; set; }
        public DependabotRoot? DependabotRoot { get; set; }
        public List<string> DependabotRecommendations { get; set; }
        public BranchProtectionPolicy? BranchPolicies { get; set; }
        public List<string> BranchPoliciesRecommendations { get; set; }
        public List<string> GitVersion { get; set; }
        public List<string> GitVersionRecommendations { get; set; }
        public List<Framework> DotNetFrameworks { get; set; }
        public List<string> DotNetFrameworksRecommendations { get; set; }
        public List<PullRequest> PullRequests { get; set; }
        public Release Release { get; set; }
        public string? LastCommitSha { get; set; }
        public DateTime LastUpdated { get; set; }
        public CoverallsCodeCoverage CoverallsCodeCoverage { get; set; }
        
        public string OwnerRepo
        {
            get
            {
                return Owner + "_" + Repo;
            }
        }

        public int TotalRecommendationCount
        {
            get
            {
                return RepoSettingsRecommendations.Count +
                    ActionRecommendations.Count +
                    DependabotRecommendations.Count +
                    BranchPoliciesRecommendations.Count +
                    GitVersionRecommendations.Count +
                    DotNetFrameworksRecommendations.Count;
            }
        }

        public DORASummaryItem? DORASummary { get; set; }
    }
}
