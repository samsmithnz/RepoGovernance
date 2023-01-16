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

        public List<UserOwnerRepo> GetRepos(string owner)
        {
            return SummaryItemsDA.GetRepos(owner);
        }

        /// <summary>
        /// Update a target of summary item
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <param name="owner">The owner or organization</param>
        /// <param name="repo">the repository being updated</param>
        /// <returns></returns>
        [HttpGet("UpdateSummaryItem")]
        public async Task<int> UpdateSummaryItem(string user, string owner, string repo)
        {
            return await SummaryItemsDA.UpdateSummaryItem(
               Configuration["AppSettings:GitHubClientId"],
               Configuration["AppSettings:GitHubClientSecret"],
               Configuration["AppSettings:CosmosDBConnectionString"],//Configuration["AppSettings:StorageConnectionString"],
               Configuration["AppSettings:DevOpsServiceURL"],
               user, owner, repo);
        }

        /// <summary>
        /// Get a list of summary item
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <returns></returns>
        [HttpGet("GetSummaryItems")]
        public async Task<List<SummaryItem>> GetSummaryItems(string user)
        {
            return await SummaryItemsDA.GetSummaryItems(
                Configuration["AppSettings:CosmosDBConnectionString"], //Configuration["AppSettings:StorageConnectionString"],
                user);
        }


        [HttpGet("ApproveSummaryItemPRs")]
        public async Task<bool> ApproveSummaryItemPRs(//string user, 
            string owner, string repo, string approver)
        {
            return await SummaryItemsDA.ApproveSummaryItemPRs(
               Configuration["AppSettings:GitHubClientId"],
               Configuration["AppSettings:GitHubClientSecret"],
               //Configuration["AppSettings:StorageConnectionString"],
               //Configuration["AppSettings:DevOpsServiceURL"],
               //user, 
               owner, repo, approver);
        }
    }
}
