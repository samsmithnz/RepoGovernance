using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public static class CoverallsCodeCoverageAPI
    {
        public async static Task<CoverallsCodeCoverage?> GetCoverallsCodeCoverage(string owner, string repo)
        {
            string url = $"https://coveralls.io/github/{owner}/{repo}.json";
            return await BaseAPI.GetResponse<CoverallsCodeCoverage>(new(), url, true);
        }

    }
}
