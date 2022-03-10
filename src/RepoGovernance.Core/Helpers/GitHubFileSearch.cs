//using RepoAutomation.Core.Models;
//using RepoGovernance.Core.APIAccess;
//using RepoGovernance.Core.Models;

//namespace RepoGovernance.Core.Helpers
//{
//    public static class GitHubFileSearch
//    {
//        public async static Task<List<string>?> SearchForFiles(string? clientId, string? secret,
//            string owner, string repository, string? file, string? extension, string path)
//        {
//            GitHubFile[]? searchResult = await RepoAutomation.Core.APIAccess.GitHubAPIAccess.GetFiles(clientId, secret,
//                owner, repository, path);

//            List<string> results = new();
//            if (searchResult == null)
//            {
//                return null;
//            }
//            else
//            {
//                foreach (GitHubFile gitHubFile in searchResult)
//                {
//                    if (file != null && gitHubFile.name == file)
//                    {
//                        results.Add(gitHubFile.name);
//                    }
//                    else if (extension != null && gitHubFile.name != null)
//                    {
//                        string[] splitFileName = gitHubFile.name.Split(".");
//                        if (splitFileName.Length > 0 && splitFileName[^1] == extension)
//                        {
//                            results.Add(gitHubFile.name);
//                        }
//                    }
//                    else if (file == null && extension == null)
//                    {
//                        if (gitHubFile != null)
//                        {
//                            results.Add(gitHubFile?.name);
//                        }
//                    }
//                }
//            }

//            return results;
//        }
//    }
//}