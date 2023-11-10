﻿namespace RepoGovernance.Core.Models.NuGetPackages
{
    public class NugetResult
    {
        public string Path { get; set; }
        public string Framework { get; set; }
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string Severity { get; set; }
    }
}
