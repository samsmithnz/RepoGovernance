namespace RepoGovernance.Core.Models.NuGetPackages
{
    /// <summary>
    /// Contains the results of a comprehensive NuGet package scan
    /// </summary>
    public class NugetScanResults
    {
        /// <summary>
        /// JSON output from 'dotnet list package --deprecated --format json'
        /// </summary>
        public string DeprecatedJson { get; set; } = string.Empty;

        /// <summary>
        /// JSON output from 'dotnet list package --outdated --format json'
        /// </summary>
        public string OutdatedJson { get; set; } = string.Empty;

        /// <summary>
        /// JSON output from 'dotnet list package --vulnerable --format json'
        /// </summary>
        public string VulnerableJson { get; set; } = string.Empty;
    }
}