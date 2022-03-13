using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.Helpers
{
    public class DotNetRepoScanner
    {
        public async static Task<List<Project>> ScanRepo(string clientId, string clientSecret, string owner, string repo)
        {
            //Search GitHub Repo
            string extension = "csproj";
            SearchResult? searchResult = await GitHubAPIAccess.SearchFiles(clientId, clientSecret, owner, repo, extension);

            //Get the content for each file
            if (searchResult == null)
            {
                return new();
            }
            else
            {
                List<Project> projects = new();

                //Scan the file content for the framework version
                foreach (SearchItem? item in searchResult.items)
                {
                    GitHubFile? gitHubFile = await GitHubAPIAccess.GetFile(clientId, clientSecret, owner, repo, item.path);
                    if (gitHubFile != null)
                    {
                        Project project = new()
                        {
                            FileName = gitHubFile?.name,
                            Path = gitHubFile?.path,
                            Content = gitHubFile?.content,
                            Framework = ""
                        };
                        projects.Add(project);
                    }
                }

                //Update framework for each project
                foreach (Project project in projects)
                {
                    string? framework = ProcessDotNetProjectFile(project, "csharp");
                    project.Framework = framework;
                }
                return projects;
            }
        }

        private string GetFrameworkFamily(string framework)
        {
            if (framework == null)
            {
                return null;
            }
            else if (framework.StartsWith("netcoreapp"))
            {
                return ".NET Core";
            }
            else if (framework.StartsWith("netstandard"))
            {
                return ".NET Standard";
            }
            else if (framework.StartsWith("v1."))
            {
                return ".NET Framework";
            }
            else if (framework.StartsWith("v2."))
            {
                return ".NET Framework";
            }
            else if (framework.StartsWith("v3."))
            {
                return ".NET Framework";
            }
            else if (framework.StartsWith("v4.") || framework.StartsWith("net4"))
            {
                return ".NET Framework";
            }
            else if (framework.StartsWith("net")) //net5.0, net6.0, etc
            {
                return ".NET";
            }
            else if (framework.StartsWith("vb6"))
            {
                return "Visual Basic 6";
            }
            else
            {
                return null;
            }
        }

        //Process .NET Framework and Core project files
        private static string? ProcessDotNetProjectFile(Project project, string language)
        {
            string? framework = null;
            string[] lines = project.Content.Split('\n');

            //scan the project file to identify the framework
            foreach (string line in lines)
            {
                if (line.IndexOf("<TargetFrameworkVersion>") > 0)
                {
                    framework = line.Replace("<TargetFrameworkVersion>", "").Replace("</TargetFrameworkVersion>", "").Trim();
                    break;
                }
                else if (line.IndexOf("<TargetFramework>") > 0)
                {
                    framework = line.Replace("<TargetFramework>", "").Replace("</TargetFramework>", "").Trim();
                    break;
                }
                else if (line.IndexOf("<TargetFrameworks>") > 0)
                {
                    string frameworks = line.Replace("<TargetFrameworks>", "").Replace("</TargetFrameworks>", "").Trim();
                    string[] frameworkList = frameworks.Split(';');
                    for (int i = 0; i < frameworkList.Length - 1; i++)
                    {
                        if (i == 0)
                        {
                            framework = frameworkList[i];
                        }
                        else
                        {
                            //Create a list
                            framework += "," + frameworkList[i];
                        }
                    }
                    break;
                }
                else if (line.IndexOf("<ProductVersion>") > 0)
                {
                    //Since product version could appear first in the list, and we could still find a target version, don't break out of the loop
                    framework = GetHistoricalFrameworkVersion(line);
                }
                else if (line.IndexOf("ProductVersion = ") > 0)
                {
                    //Since product version could appear first in the list, and we could still find a target version, don't break out of the loop
                    framework = GetHistoricalFrameworkVersion(line);
                }
            }
            return framework;
        }

        //Really old frameworks are tied to the Visual Studio version, and it's not a 1:1 connection.
        //Luckily most projects won't be here
        private static string? GetHistoricalFrameworkVersion(string line)
        {
            string productVersion = line.Replace("<ProductVersion>", "").Replace("</ProductVersion>", "").Replace("ProductVersion = ", "").Replace("\"", "").Trim();
            //https://en.wikipedia.org/wiki/Microsoft_Visual_Studio#History
            //+---------------------------+---------------+-----------+----------------+
            //|       Product name        |   Codename    | Version # | .NET Framework | 
            //+---------------------------+---------------+-----------+----------------+
            //| Visual Studio .NET (2002) | Rainier       | 7.0.*     | 1              |
            //| Visual Studio .NET 2003   | Everett       | 7.1.*     | 1.1            |
            //| Visual Studio 2005        | Whidbey       | 8.0.*     | 2.0, 3.0       |
            //| Visual Studio 2008        | Orcas         | 9.0.*     | 2.0, 3.0, 3.5  |
            //| Visual Studio 2010        | Dev10/Rosario | 10.0.*    | 2.0 – 4.0      |
            //| Visual Studio 2012        | Dev11         | 11.0.*    | 2.0 – 4.5.2    |
            //| Visual Studio 2013        | Dev12         | 12.0.*    | 2.0 – 4.5.2    |
            //| Visual Studio 2015        | Dev14         | 14.0.*    | 2.0 – 4.6      |
            //+---------------------------+---------------+-----------+----------------+

            //Only process the earliest Visual Studio's, as the later versions should be picked up by the product version
            if (productVersion.StartsWith("7.0") == true)
            {
                return "v1.0";
            }
            else if (productVersion.StartsWith("7.1") == true)
            {
                return "v1.1";
            }
            else if (productVersion.StartsWith("8.0") == true)
            {
                return "v2.0";
            }
            else
            {
                return null;
            }
        }
    }
}