﻿using RepoAutomation.Core.Models;

namespace RepoGovernance.Core.Models
{
    public class SummaryItem
    {
        public SummaryItem(string repo)
        {
            Repo = repo;
            _actions = new();
            ActionRecommendations = new();
            _dependabot = new();
            DependabotRecommendations = new();
            BranchPolicies = new();
            BranchPoliciesRecommendations = new();
            GitVersion = new();
            GitVersionRecommendations = new();
            Frameworks = new();
            FrameworksRecommendations = new();
        }

        private List<string> _actions;
        private List<string> _dependabot;

        public string Repo { get; internal set; }
        public List<string> Actions
        {
            get
            {
                return _actions;
            }
            set
            {
                _actions = value;
                if (_actions != null && _actions.Count == 0)
                {
                    ActionRecommendations.Add("Consider adding an action to build your project");
                }
            }
        }
        public List<string> ActionRecommendations { get; set; }
        public List<string> Dependabot
        {
            get
            {
                return _dependabot;
            }
            set
            {
                _dependabot = value;
                if (_dependabot != null)
                {
                    if (_dependabot.Count == 0)
                    {
                        DependabotRecommendations.Add("Consider adding a Dependabot file to automatically update dependencies");
                    }
                    else if (_dependabot.Count > 1)
                    {
                        DependabotRecommendations.Add("Consider consilidating your Dependabot files to just one file");
                    }
                    else if (_dependabot.Count == 1)
                    {
                        //Scan the dependabot file
                    }
                }
            }
        }
        public GitHubFile DependabotFile { get; set; }
        public List<string> DependabotRecommendations { get; set; }
        public BranchProtectionPolicy? BranchPolicies { get; set; }
        public List<string> BranchPoliciesRecommendations { get; set; }
        public List<string> GitVersion { get; set; }
        public List<string> GitVersionRecommendations { get; set; }
        public List<string> Frameworks { get; set; }
        public List<string> FrameworksRecommendations { get; set; }
    }
}
