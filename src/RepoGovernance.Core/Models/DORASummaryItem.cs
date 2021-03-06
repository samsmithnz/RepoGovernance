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
            DeploymentFrequencyBadgeWithMetricURL = "https://img.shields.io/badge/Deployment%20frequency%20%280.00%20times%20per%20year%29-None-lightgrey";
            LeadTimeForChangesBadgeURL = "https://img.shields.io/badge/Lead%20time%20for%20changes-None-lightgrey";
            LeadTimeForChangesWithMetricURL = "https://img.shields.io/badge/Lead%20time%20for%20changes%20%280.0%20hours%29-None-lightgrey";
            MeanTimeToRestoreBadgeURL = "https://img.shields.io/badge/Time%20to%20restore%20service-None-lightgrey";
            MeanTimeToRestoreBadgeWithMetricURL = "https://img.shields.io/badge/Time%20to%20restore%20service%20%280.00%20hours%29-None-lightgrey";
            ChangeFailureRateBadgeURL = "https://img.shields.io/badge/Change%20failure%20rate-None-lightgrey";
            ChangeFailureRateBadgeWithMetricURL = "https://img.shields.io/badge/Change%20failure%20rate%20%28100.00%25%29-None-lightgrey";
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
