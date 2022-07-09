namespace RepoGovernance.Core.Models
{
    //This class will mirror the result from DevOpsMetrics
    public class DORASummaryItem
    {
        public DORASummaryItem(string owner, string repo)
        {
            Owner = owner;
            Repo = repo;
            //Set the properties to defaults (none)
            DeploymentFrequencyBadgeURL = "https://img.shields.io/badge/Deployment%20frequency-None-lightgrey";
            //DeploymentFrequencyBadgeWithMetricURL = deploymentFrequencyBadgeWithMetricURL;
            LeadTimeForChangesBadgeURL = "https://img.shields.io/badge/Lead%20time%20for%20changes-None-lightgrey";
            //LeadTimeForChangesWithMetricURL = leadTimeForChangesWithMetricURL;
            MeanTimeToRestoreBadgeURL = "https://img.shields.io/badge/Time%20to%20restore%20service-None-lightgrey";
            //MeanTimeToRestoreBadgeWithMetricURL = meanTimeToRestoreBadgeWithMetricURL;
            ChangeFailureRateBadgeURL = "https://img.shields.io/badge/Change%20failure%20rate-None-lightgrey";
            //ChangeFailureRateBadgeWithMetricURL = changeFailureRateBadgeWithMetricURL;
        }

        public string Owner
        {
            get; set;
        }
        public string Repo
        {
            get; set;
        }
        public string DeploymentFrequencyBadgeURL
        {
            get; set;
        }
        public string DeploymentFrequencyBadgeWithMetricURL
        {
            get; set;
        }
        public string LeadTimeForChangesBadgeURL
        {
            get; set;
        }
        public string LeadTimeForChangesWithMetricURL
        {
            get; set;
        }
        public string MeanTimeToRestoreBadgeURL
        {
            get; set;
        }
        public string MeanTimeToRestoreBadgeWithMetricURL
        {
            get; set;
        }
        public string ChangeFailureRateBadgeURL
        {
            get; set;
        }
        public string ChangeFailureRateBadgeWithMetricURL
        {
            get; set;
        }
    }
}
