using Newtonsoft.Json;
using RepoGovernance.Core.Models.NuGetPackages;
using System.Diagnostics;
using Process = System.Diagnostics.Process;

namespace RepoGovernance.Core.Helpers
{
    public class DotNetPackages
    {
        public List<NugetResult> GetNugetPackagesOutdated(string path)
        {
            List<NugetResult> results = new();

            Process process = new();
            ProcessStartInfo startInfo = new()
            {
                FileName = "dotnet.exe",
                Arguments = "list package --outdated --format json",
                WorkingDirectory = path,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            process.StartInfo = startInfo;
            process.Start();


            string output = process.StandardOutput.ReadToEnd();
            Root? root = JsonConvert.DeserializeObject<Root>(output);
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
                                results.Add(new NugetResult()
                                {
                                    Path = project.path,
                                    Framework = framework.framework,
                                    PackageId = package.id,
                                    PackageVersion = package.latestVersion
                                });
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}
