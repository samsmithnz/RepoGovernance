using Newtonsoft.Json;
using RepoGovernance.Core.Models.NuGetPackages;
using System.Diagnostics;
using Process = System.Diagnostics.Process;

namespace RepoGovernance.Core.Helpers
{
    public class DotNetPackages
    {
        public List<NugetPackage> GetNugetPackagesDeprecated(string json)
        {
            List<NugetPackage> results = new();

            //Process the output
            Root? root = JsonConvert.DeserializeObject<Root>(json);
            if (root != null && root.Projects != null && root.Projects.Count > 0)
            {
                foreach (Project project in root.Projects)
                {
                    if (project.frameworks != null)
                    {
                        foreach (Framework framework in project.frameworks)
                        {
                            foreach (Package package in framework.topLevelPackages)
                            {
                                results.Add(new NugetPackage()
                                {
                                    Path = project.path,
                                    Framework = framework.framework,
                                    PackageId = package.id,
                                    PackageVersion = package.requestedVersion,
                                    Type = "Deprecated"
                                });
                            }
                        }
                    }
                }
            }

            return results;
        }

        public List<NugetPackage> GetNugetPackagesOutdated(string json)
        {
            List<NugetPackage> results = new();
            
            //Process the output
            Root? root = JsonConvert.DeserializeObject<Root>(json);
            if (root != null && root.Projects != null && root.Projects.Count > 0)
            {
                foreach (Project project in root.Projects)
                {
                    if (project.frameworks != null)
                    {
                        foreach (Framework framework in project.frameworks)
                        {
                            foreach (Package package in framework.topLevelPackages)
                            {
                                results.Add(new NugetPackage()
                                {
                                    Path = project.path,
                                    Framework = framework.framework,
                                    PackageId = package.id,
                                    PackageVersion = package.latestVersion,
                                    Type = "Outdated"
                                });
                            }
                        }
                    }
                }
            }

            return results;
        }

        public List<NugetPackage> GetNugetPackagesVulnerable(string json)
        {
            List<NugetPackage> results = new();

            //Process the output
            Root? root = JsonConvert.DeserializeObject<Root>(json);
            if (root != null && root.Projects != null && root.Projects.Count > 0)
            {
                foreach (Project project in root.Projects)
                {
                    if (project.frameworks != null)
                    {
                        foreach (Framework framework in project.frameworks)
                        {
                            foreach (Package package in framework.topLevelPackages)
                            {
                                results.Add(new NugetPackage()
                                {
                                    Path = project.path,
                                    Framework = framework.framework,
                                    PackageId = package.id,
                                    PackageVersion = package.requestedVersion,
                                    Severity = package.GetFirstVulnerability(),
                                    Type = "Vulnerable"
                                });
                            }
                        }
                    }
                }
            }

            return results;
        }

        public string GetProcessOutput(string path, string arguments)
        {
            Process process = new();
            ProcessStartInfo startInfo = new()
            {
                FileName = "dotnet", // Use "dotnet" instead of "dotnet.exe" for cross-platform compatibility
                Arguments = arguments,
                WorkingDirectory = path,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            process.StartInfo = startInfo;
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// Runs 'dotnet list package --deprecated --format json' and returns the raw JSON output
        /// </summary>
        /// <param name="path">Path to the directory containing the solution or project files</param>
        /// <returns>JSON string containing deprecated package information</returns>
        public string GetNugetPackagesDeprecatedJson(string path)
        {
            return GetProcessOutput(path, "list package --deprecated --format json");
        }

        /// <summary>
        /// Runs 'dotnet list package --outdated --format json' and returns the raw JSON output
        /// </summary>
        /// <param name="path">Path to the directory containing the solution or project files</param>
        /// <returns>JSON string containing outdated package information</returns>
        public string GetNugetPackagesOutdatedJson(string path)
        {
            return GetProcessOutput(path, "list package --outdated --format json");
        }

        /// <summary>
        /// Runs 'dotnet list package --vulnerable --format json' and returns the raw JSON output
        /// </summary>
        /// <param name="path">Path to the directory containing the solution or project files</param>
        /// <returns>JSON string containing vulnerable package information</returns>
        public string GetNugetPackagesVulnerableJson(string path)
        {
            return GetProcessOutput(path, "list package --vulnerable --format json");
        }

        /// <summary>
        /// Runs 'dotnet restore' to restore NuGet packages before scanning
        /// </summary>
        /// <param name="path">Path to the directory containing the solution or project files</param>
        /// <returns>Output from the restore command</returns>
        public string RestorePackages(string path)
        {
            return GetProcessOutput(path, "restore");
        }

        /// <summary>
        /// Comprehensive NuGet package scanning that mimics the GitHub Action workflow
        /// Runs restore, then scans for deprecated, outdated, and vulnerable packages
        /// </summary>
        /// <param name="path">Path to the directory containing the solution or project files</param>
        /// <returns>Object containing all scan results</returns>
        public NugetScanResults ScanAllPackages(string path)
        {
            // First restore packages
            RestorePackages(path);

            // Run all three scans
            return new NugetScanResults
            {
                DeprecatedJson = GetNugetPackagesDeprecatedJson(path),
                OutdatedJson = GetNugetPackagesOutdatedJson(path),
                VulnerableJson = GetNugetPackagesVulnerableJson(path)
            };
        }
    }
}
