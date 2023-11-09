using Newtonsoft.Json;
using RepoGovernance.Core.Models.NuGetPackages;
using System.Diagnostics;
using Process = System.Diagnostics.Process;

namespace RepoGovernance.Core.Helpers
{
    public class DotNetPackages
    {
        public List<string> GetNugetPackagesOutdated(string path)
        {
            List<string> result = new();

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
                    foreach (Framework framework in project.frameworks)
                    {
                        foreach (Package package in framework.topLevelPackages)
                        {
                            result.Add(project.path + ";" + package.id + ";" + package.latestVersion);
                        }
                    }
                }
            }

            return result;
        }
    }
}
