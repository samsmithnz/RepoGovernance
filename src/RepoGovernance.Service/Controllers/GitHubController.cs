using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        //public async Task<List<Repo>> GetRepos(string owner)
        //{
        //    RepoGovernance.Core.APIAccess.GitHubApiAccess.GetRepo();
        //}
        [HttpGet("GetRepos")]
        public List<UserOwnerRepo> GetRepos(string user)
        {
            return DatabaseAccess.GetRepos(user);
        }
    }
}
