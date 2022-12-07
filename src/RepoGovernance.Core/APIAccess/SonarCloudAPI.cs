using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public static class SonarCloudAPI
    {
        public async static Task<SonarCloud?> GetSonarCloudCodeSmells(string owner, string repo)
        {
            string[] metrics = new string[] { "code_smells", "ncloc" };
            string url = $"https://sonarcloud.io/api/project_badges/measure?project={owner}_{repo}&metric=code_smells";
            string linkCodeSmells = $"https://sonarcloud.io/project/issues?resolved=false&id={owner}_{repo}";
            string linkLOC = $"https://sonarcloud.io/component_measures?metric=ncloc&id={owner}_{repo}";
            
            
            return await BaseAPI.GetResponse<SonarCloud>(new(), url, true);
        }

    }
}
