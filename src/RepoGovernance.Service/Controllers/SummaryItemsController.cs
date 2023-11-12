using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;
using RepoGovernance.Service.Models;

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

        [HttpGet("GetRepos")]
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
                user, owner, repo,
                Configuration["AppSettings:AzureTenantId"],
                Configuration["AppSettings:AzureClientId"],
                Configuration["AppSettings:AzureClientSecret"]);
        }

        [HttpPost("UpdateSummaryItemNuGetPackageStats")]
        public async Task<int> UpdateSummaryItemNuGetPackageStats(NuGetPayload nugetPayload)
        {
            if (nugetPayload != null)
            {
                string? repo = nugetPayload?.Repo;
                string? owner = nugetPayload?.Owner;
                string? user = nugetPayload?.User;
                string? jsonPayload = nugetPayload?.JsonPayload;
                string? payloadType = nugetPayload?.PayloadType;

                if (repo == null || owner == null || user == null || jsonPayload == null)
                {
                    return -1;
                }
                return await SummaryItemsDA.UpdateSummaryItemNuGetPackageStats(
                    Configuration["AppSettings:CosmosDBConnectionString"],
                    user, owner, repo,
                    jsonPayload, payloadType);
            }
            else
            {
                return -1;
            }
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

        /// <summary>
        /// Get a summary item
        /// </summary>
        /// <param name="owner">the owner or organization</param>
        /// <param name="repo">the repo</param>
        /// <returns></returns>
        [HttpGet("GetSummaryItem")]
        public async Task<SummaryItem?> GetSummaryItem(string user, string owner, string repo)
        {
            return await SummaryItemsDA.GetSummaryItem(
                Configuration["AppSettings:CosmosDBConnectionString"],
                user, owner, repo);
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
