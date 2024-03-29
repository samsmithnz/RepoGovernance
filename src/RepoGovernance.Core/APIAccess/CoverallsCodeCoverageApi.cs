﻿using RepoGovernance.Core.Models;

namespace RepoGovernance.Core.APIAccess
{
    public static class CoverallsCodeCoverageApi
    {
        public async static Task<CoverallsCodeCoverage?> GetCoverallsCodeCoverage(string owner, string repo)
        {
            string url = $"https://coveralls.io/github/{owner}/{repo}.json";
            return await BaseApi.GetResponse<CoverallsCodeCoverage>(new(), url, true);
        }

    }
}
