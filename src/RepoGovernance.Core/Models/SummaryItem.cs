﻿using GitHubActionsDotNet.Models.Dependabot;
using RepoAutomation.Core.Models;

namespace RepoGovernance.Core.Models
{
    public class SummaryItem
    {
        public SummaryItem(string repo)
        {
            Repo = repo;
            Actions = new();
            ActionRecommendations = new();
            Dependabot = new();
            DependabotRecommendations = new();
            BranchPolicies = new();
            BranchPoliciesRecommendations = new();
            GitVersion = new();
            GitVersionRecommendations = new();
            Frameworks = new();
            FrameworksRecommendations = new();
        }

        public string Repo { get; internal set; }
        public List<string> Actions { get; set; }
        public List<string> ActionRecommendations { get; set; }
        public List<string> Dependabot { get; set; }
        public GitHubFile? DependabotFile { get; set; }
        public DependabotRoot? DependabotRoot { get; set; }
        public List<string> DependabotRecommendations { get; set; }
        public BranchProtectionPolicy BranchPolicies { get; set; }
        public List<string> BranchPoliciesRecommendations { get; set; }
        public List<string> GitVersion { get; set; }
        public List<string> GitVersionRecommendations { get; set; }
        public List<string> Frameworks { get; set; }
        public List<string> FrameworksRecommendations { get; set; }
    }
}
