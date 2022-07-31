using RepoAutomation.Core.APIAccess;
using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.Helpers
{
    public class DotNetRepoScanner
    {
        public async static Task<List<Project>> ScanRepo(string? clientId, string? clientSecret, string owner, string repo)
        {
            List<Project> projects = new();

            //Search GitHub Repo for various extensions
            //C#/ *.csproj
            projects.AddRange(await SearchForFiles(clientId, clientSecret, owner, repo, "csproj"));
            //Unity/ ProjectVersion.txt
            projects.AddRange(await SearchForFiles(clientId, clientSecret, owner, repo, null, "ProjectVersion.txt"));

            return projects;
        }

        private async static Task<List<Project>> SearchForFiles(string? clientId, string? clientSecret, string owner, string repo, string? extension = null, string? fileName = null)
        {
            SearchResult? searchResult = await GitHubAPIAccess.SearchFiles(clientId, clientSecret, owner, repo, extension, fileName);
            //Get the content for each file
            if (searchResult == null)
            {
                return new();
            }
            else
            {
                List<Project> projects = new();

                //Scan the file content for the framework version
                if (searchResult.items != null)
                {
                    foreach (SearchItem? item in searchResult.items)
                    {
                        if (item.path != null)
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
                    }
                }

                //If there are multiple frameworks, split them out
                List<Project> additionalProjects = new();
                foreach (Project project in projects)
                {
                    string? framework = ProcessDotNetProjectFile(project);

                    if (framework != null && framework?.IndexOf(",") > 0)
                    {
                        string[] frameworks = framework.Split(',');
                        for (int i = 0; i < frameworks.Length - 1; i++)
                        {
                            if (i == 0)
                            {
                                project.Framework = frameworks[0];
                            }
                            else
                            {
                                Project additionalProject = new()
                                {
                                    FileName = project.FileName,
                                    Path = project.Path,
                                    Framework = frameworks[i],
                                };
                                additionalProjects.Add(additionalProject);
                            }
                        }
                    }
                    else
                    {
                        project.Framework = framework;
                    }
                }
                projects.AddRange(additionalProjects);

                //Update colors for each project
                foreach (Project project in projects)
                {
                    project.Color = GetColor(project.Framework);
                }
                return projects;
            }
        }

        private static string GetFrameworkFamily(string? framework)
        {
            if (framework == null)
            {
                return "";
            }
            //else if (framework.StartsWith("netcoreapp"))
            //{
            //    return ".NET Core";
            //}
            //else if (framework.StartsWith("netstandard"))
            //{
            //    return ".NET Standard";
            //}
            else if (framework.StartsWith("v1."))
            {
                return ".NET Framework " + framework;
            }
            else if (framework.StartsWith("v2."))
            {
                return ".NET Framework " + framework;
            }
            else if (framework.StartsWith("v3."))
            {
                return ".NET Framework " + framework;
            }
            else if (framework.StartsWith("v4.") || framework.StartsWith("net4"))
            {
                return ".NET Framework " + framework;
            }
            //else if (framework.StartsWith("vb6"))
            //{
            //    return "Visual Basic 6";
            //}
            //else if (framework.StartsWith("net")) //net5.0, net6.0, etc
            //{
            //    return ".NET";
            //}
            else
            {
                return framework;
            }
        }

        //Process .NET Framework and Core project files
        private static string? ProcessDotNetProjectFile(Project project)
        {
            string? framework = null;
            if (project.Content != null)
            {
                string[] lines = project.Content.Split('\n');

                //scan the project file to identify the framework
                foreach (string line in lines)
                {
                    if (line.Contains("<TargetFrameworkVersion>"))
                    {
                        //A single target framework version (older way it appears)
                        framework = line.Replace("<TargetFrameworkVersion>", "").Replace("</TargetFrameworkVersion>", "").Trim();
                        framework = GetFrameworkFamily(framework);
                        break;
                    }
                    else if (line.Contains("<TargetFramework>"))
                    {
                        //A single target framework version (newer way it appears)
                        framework = line.Replace("<TargetFramework>", "").Replace("</TargetFramework>", "").Trim();
                        framework = GetFrameworkFamily(framework);
                        break;
                    }
                    else if (line.Contains("<TargetFrameworks>"))
                    {
                        //Multiple versions exist, split them out.
                        string frameworks = line.Replace("<TargetFrameworks>", "").Replace("</TargetFrameworks>", "").Trim();
                        string[] frameworkList = frameworks.Split(';');
                        for (int i = 0; i < frameworkList.Length - 1; i++)
                        {
                            if (i > 0)
                            {
                                framework += ",";
                            }
                            framework += GetFrameworkFamily(frameworkList[i]);
                        }
                        break;
                    }
                    else if (line.Contains("<ProductVersion>") || line.Contains("ProductVersion = "))
                    {
                        //Since product version could appear first in the list, and we could still find a target version, don't break out of the loop
                        framework = GetFrameworkFamily(GetHistoricalFrameworkVersion(line));
                    }
                    else if (line.Contains("m_EditorVersion:"))
                    {
                        //Unity project file
                        framework = GetUnityFrameworkVersion(line);
                    }
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
            //Note that this may not be entirely accurate - for example, VS2008 could ignore a .NET 3 version, but these should be pretty rare - even if it misidentifies .NET framework 1/2/3 - the story is the same - these are wildly obsolete and need to be resolved.
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

        private static string? GetUnityFrameworkVersion(string line)
        {
            //An example of what to expect:
            //m_EditorVersion: 2020.3.12f1
            //m_EditorVersionWithRevision: 2020.3.12f1(b3b2c6512326)
            string fullVersion = line.Replace("m_EditorVersion:", "").Trim();
            string[] splitVersion = fullVersion.Split('.');
            string unityVersion = "Unity3d v" + splitVersion[0] + "." + splitVersion[1];

            return unityVersion;
        }

        private static string? GetColor(string? framework)
        {
            if (framework == null)
            {
                //Unknown/gray
                return "bg-secondary";
            }
            else if (framework.Contains(".NET Framework v1") ||
                framework.Contains(".NET Framework v2") ||
                framework.Contains(".NET Framework v3") ||
                framework.Contains(".NET Framework v4.0") ||
                framework.Contains(".NET Framework v4.1") ||
                framework.Contains(".NET Framework v4.2") ||
                framework.Contains(".NET Framework v4.3") ||
                framework.Contains(".NET Framework v4.4") ||
                framework.Contains(".NET Framework v4.5") ||
                framework.Contains(".NET Framework v4.6.1") ||
                framework.Contains("net5.0"))
            {
                //Unsupported/End of life/red
                return "bg-danger";
            }
            else if (framework.Contains(".NET Framework") ||
                framework.Contains("netcoreapp"))
            {
                //Supported, but old/orange
                return "bg-info";
            }
            else if (framework.Contains("net6.0") ||
                framework.Contains("netstandard") ||
                framework.Contains("Unity3d v2020") ||
                framework.Contains(".NET Framework v4.6.2") ||
                framework.Contains(".NET Framework v4.7") ||
                framework.Contains(".NET Framework v4.8"))
            {
                //Supported/Ok/blue
                return "bg-primary";
            }
            else
            {
                //Unknown/gray
                return "bg-secondary";
            }
        }
    }
}