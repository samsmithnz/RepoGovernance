using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public static class SonarCloudAPI
    {
        public async static Task<SonarCloud?> GetSonarCloudMetrics(string owner, string repo)
        {
            string? codeSmellsBadgeImage = await GetSonarCloudCodeSmells(owner, repo);
            string? linesOfCodeBadgeImage = await GetSonarCloudLinesOfCode(owner, repo);

            return new SonarCloud()
            {
                CodeSmellsBadgeImage = codeSmellsBadgeImage,
                CodeSmellsLink = $"https://sonarcloud.io/project/issues?resolved=false&id={owner}_{repo}",
                LinesOfCodeBadgeImage = linesOfCodeBadgeImage,
                LinesOfCodeLink = $"https://sonarcloud.io/component_measures?metric=ncloc&id={owner}_{repo}"
            };
        }

        public async static Task<string?> GetSonarCloudCodeSmells(string owner, string repo)
        {
            string url = $"https://sonarcloud.io/api/project_badges/measure?project={owner}_{repo}&metric=code_smells";
            return await BaseAPI.GetResponse<string>(new(), url, true);
        }

        public async static Task<string?> GetSonarCloudLinesOfCode(string owner, string repo)
        {
            string url = $"https://sonarcloud.io/api/project_badges/measure?project={owner}_{repo}&metric=ncloc";
            return await BaseAPI.GetResponse<string>(new(), url, true);
        }

    }
}
