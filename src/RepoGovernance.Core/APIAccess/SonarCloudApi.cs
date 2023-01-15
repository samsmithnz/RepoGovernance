using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public static class SonarCloudApi
    {
        public async static Task<SonarCloud?> GetSonarCloudMetrics(string owner, string repo)
        {
            string? codeSmellsBadgeImage = await GetSonarCloudCodeSmells(owner, repo);
            string? bugsBadgeImage = await GetSonarCloudCodeBugs(owner, repo);
            string? linesOfCodeBadgeImage = await GetSonarCloudLinesOfCode(owner, repo);

            SonarCloud? sonarCloud = new();
            if (string.IsNullOrEmpty(codeSmellsBadgeImage) == false)
            {
                sonarCloud.CodeSmellsBadgeImage = codeSmellsBadgeImage;
                sonarCloud.CodeSmellsLink = $"https://sonarcloud.io/project/issues?resolved=false&types=CODE_SMELL&id={owner}_{repo}";
            }
            if (string.IsNullOrEmpty(bugsBadgeImage) == false)
            {
                sonarCloud.BugsBadgeImage = bugsBadgeImage;
                sonarCloud.BugsLink = $"https://sonarcloud.io/project/issues?resolved=false&types=BUG&id={owner}_{repo}";
            }
            if (string.IsNullOrEmpty(linesOfCodeBadgeImage) == false)
            {
                sonarCloud.LinesOfCodeBadgeImage = linesOfCodeBadgeImage;
                sonarCloud.LinesOfCodeLink = $"https://sonarcloud.io/component_measures?metric=ncloc&id={owner}_{repo}";
            }
            //If there were no links, return null
            if (string.IsNullOrEmpty(codeSmellsBadgeImage) == true &&
                string.IsNullOrEmpty(bugsBadgeImage) == true &&
                string.IsNullOrEmpty(linesOfCodeBadgeImage) == true)
            {
                sonarCloud = null;
            }
            return sonarCloud;
        }

        public async static Task<string?> GetSonarCloudCodeSmells(string owner, string repo)
        {
            string url = $"https://sonarcloud.io/api/project_badges/measure?project={owner}_{repo}&metric=code_smells";
            return await GetResponseString(new(), url, true);
        }

        public async static Task<string?> GetSonarCloudCodeBugs(string owner, string repo)
        {
            string url = $"https://sonarcloud.io/api/project_badges/measure?project={owner}_{repo}&metric=bugs";
            return await GetResponseString(new(), url, true);
        }

        public async static Task<string?> GetSonarCloudLinesOfCode(string owner, string repo)
        {
            string url = $"https://sonarcloud.io/api/project_badges/measure?project={owner}_{repo}&metric=ncloc";
            return await GetResponseString(new(), url, true);
        }

        private async static Task<string?> GetResponseString(HttpClient client, string url, bool ignoreErrors = false)
        {
            string? obj = null;
            if (client != null && url != null)
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (ignoreErrors == true || response.IsSuccessStatusCode == true)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseBody) == false &&
                            responseBody.Contains("Project not found") == false &&
                            responseBody.Contains("Measure has not been found") == false)
                        {
                            obj = responseBody;
                        }
                    }
                    else
                    {
                        //Throw an exception
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            return obj;
        }

    }
}
