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
                FileName = "dotnet.exe",
                Arguments = arguments,
                WorkingDirectory = path,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            process.StartInfo = startInfo;
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }
    }
}
