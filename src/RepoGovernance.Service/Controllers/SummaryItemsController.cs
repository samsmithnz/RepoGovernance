using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryItemsController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public SummaryItemsController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<ProfileOwnerRepo> GetRepos(string owner)
        {
            return SummaryItemsDA.GetRepos(owner);
        }

        /// <summary>
        /// Get a list of summary items
        /// </summary>
        /// <param name="profile">The profile - often is also the owner, that has access to organizations</param>
        /// <param name="owner">The owner or organization</param>
        /// <param name="repo">the repository being updated</param>
        /// <returns></returns>
        [HttpGet("UpdateSummaryItems")]
        public async Task<int> UpdateSummaryItems(string profile, string owner, string repo)
        {
            return await SummaryItemsDA.UpdateSummaryItems(
               Configuration["AppSettings:GitHubClientId"],
               Configuration["AppSettings:GitHubClientSecret"],
               Configuration["AppSettings:StorageConnectionString"],
               profile, owner, repo);
        }

        [HttpGet("GetSummaryItems")]
        public List<SummaryItem> GetSummaryItems(string profile)
        {
            return SummaryItemsDA.GetSummaryItems(
                Configuration["AppSettings:StorageConnectionString"],
                profile);
        }
    }
}
