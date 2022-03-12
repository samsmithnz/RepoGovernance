using GitHubActionsDotNet.Models.Dependabot;
using RepoAutomation.Core.Models;

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
        private DependabotRoot _dependabotRoot;
        public DependabotRoot DependabotRoot
        {
            get
            {
                return _dependabotRoot;
            }
            set
            {
                _dependabotRoot = value;
                if (_dependabotRoot.updates.Count == 0)
                {
                    DependabotRecommendations.Add("Dependabot file exists, but is not configured to scan any manifest files");
                }
                int actionsCount = 0;
                foreach (Package? item in _dependabotRoot.updates)
                {
                    if (item.package_ecosystem == "github-actions")
                    {
                        actionsCount++;
                    }
                }
                if (_actions.Count > 0 && actionsCount == 0)
                {
                    DependabotRecommendations.Add("Consider adding github-actions ecosystem to Dependabot to auto-update actions dependencies");
                }
            }
        }
        public List<string> DependabotRecommendations { get; set; }
        public BranchProtectionPolicy? BranchPolicies { get; set; }
        public List<string> BranchPoliciesRecommendations { get; set; }
        public List<string> GitVersion { get; set; }
        public List<string> GitVersionRecommendations { get; set; }
        public List<string> Frameworks { get; set; }
        public List<string> FrameworksRecommendations { get; set; }
    }
}
