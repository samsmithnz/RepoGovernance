namespace RepoGovernance.Core.Models
{
    public class SecurityAlertsResult
    {
        public int CodeScanningCount { get; set; }
        public int SecretScanningCount { get; set; }
        public int TotalCount { get; set; }

        public SecurityAlertsResult(int codeScanningCount, int secretScanningCount, int totalCount)
        {
            CodeScanningCount = codeScanningCount;
            SecretScanningCount = secretScanningCount;
            TotalCount = totalCount;
        }
    }
}