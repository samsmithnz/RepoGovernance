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
        public async Task<ActionResult<List<UserOwnerRepo>>> GetRepos(string owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await SummaryItemsDA.GetRepos(Configuration["AppSettings:CosmosDBConnectionString"], owner);
        }

        /// <summary>
        /// Update a target of summary item
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <param name="owner">The owner or organization</param>
        /// <param name="repo">the repository being updated</param>
        /// <returns></returns>
        [HttpGet("UpdateSummaryItem")]
        public async Task<ActionResult<int>> UpdateSummaryItem(string user, string owner, string repo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await SummaryItemsDA.UpdateSummaryItem(
                Configuration["AppSettings:GitHubClientId"],
                Configuration["AppSettings:GitHubClientSecret"],
                Configuration["AppSettings:CosmosDBConnectionString"],//Configuration["AppSettings:StorageConnectionString"],
                Configuration["AppSettings:DevOpsServiceURL"],
                user, owner, repo,
                Configuration["AppSettings:GitHubId"],
                Configuration["AppSettings:GitHubSecret"],
                Configuration["AppSettings:AzureTenantId"],
                Configuration["AppSettings:AzureClientId"],
                Configuration["AppSettings:AzureClientSecret"]);
        }

        /// <summary>
        /// Update a target of summary item with optional NuGet package data
        /// </summary>
        /// <param name="request">The update request containing user, owner, repo, and optional NuGet payloads</param>
        /// <returns></returns>
        [HttpPost("UpdateSummaryItem")]
        public async Task<ActionResult<int>> UpdateSummaryItemWithNuGet(UpdateSummaryItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request?.User == null || request?.Owner == null || request?.Repo == null)
            {
                return BadRequest("User, Owner, and Repo are required");
            }

            return await SummaryItemsDA.UpdateSummaryItem(
                Configuration["AppSettings:GitHubClientId"],
                Configuration["AppSettings:GitHubClientSecret"],
                Configuration["AppSettings:CosmosDBConnectionString"],
                Configuration["AppSettings:DevOpsServiceURL"],
                request.User, request.Owner, request.Repo,
                Configuration["AppSettings:GitHubId"],
                Configuration["AppSettings:GitHubSecret"],
                Configuration["AppSettings:AzureTenantId"],
                Configuration["AppSettings:AzureClientId"],
                Configuration["AppSettings:AzureClientSecret"],
                null, // azureDeployment
                request.NugetDeprecatedPayload,
                request.NugetOutdatedPayload,
                request.NugetVulnerablePayload);
        }

        [HttpPost("UpdateSummaryItemNuGetPackageStats")]
        public async Task<ActionResult<int>> UpdateSummaryItemNuGetPackageStats(NuGetPayload nugetPayload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (nugetPayload != null)
            {
                string? repo = nugetPayload?.Repo;
                string? owner = nugetPayload?.Owner;
                string? user = nugetPayload?.User;
                //There is some weirdness when the json is embedded in this object and then the object is serialized a second time - it returns an array of strings.
                string? jsonPayload = nugetPayload?.JsonPayloadString;
                string? payloadType = nugetPayload?.PayloadType;

                if (repo == null || owner == null || user == null || jsonPayload == null || payloadType == null)
                {
                    return BadRequest("Repo, Owner, User, JsonPayloadString, and PayloadType are required");
                }
                return await SummaryItemsDA.UpdateSummaryItemNuGetPackageStats(
                    Configuration["AppSettings:CosmosDBConnectionString"],
                    user, owner, repo,
                    Configuration["AppSettings:GitHubId"],
                    Configuration["AppSettings:GitHubSecret"],
                    jsonPayload, payloadType);
            }
            else
            {
                return BadRequest("NuGet payload is required");
            }
        }

        /// <summary>
        /// Get a list of summary item
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <returns></returns>
        [HttpGet("GetSummaryItems")]
        public async Task<ActionResult<List<SummaryItem>>> GetSummaryItems(string user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await SummaryItemsDA.GetSummaryItems(
                Configuration["AppSettings:CosmosDBConnectionString"], //Configuration["AppSettings:StorageConnectionString"],
                user,
                Configuration["AppSettings:GitHubId"],
                Configuration["AppSettings:GitHubSecret"]);
        }

        /// <summary>
        /// Get a summary item
        /// </summary>
        /// <param name="owner">the owner or organization</param>
        /// <param name="repo">the repo</param>
        /// <returns></returns>
        [HttpGet("GetSummaryItem")]
        public async Task<ActionResult<SummaryItem?>> GetSummaryItem(string user, string owner, string repo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await SummaryItemsDA.GetSummaryItem(
                Configuration["AppSettings:CosmosDBConnectionString"],
                user, owner, repo,
                Configuration["AppSettings:GitHubId"],
                Configuration["AppSettings:GitHubSecret"]);
        }


        [HttpGet("ApproveSummaryItemPRs")]
        public async Task<ActionResult<bool>> ApproveSummaryItemPRs(//string user, 
            string owner, string repo, string approver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await SummaryItemsDA.ApproveSummaryItemPRs(
               Configuration["AppSettings:GitHubClientId"],
               Configuration["AppSettings:GitHubClientSecret"],
               owner, repo, approver);
        }

        /// <summary>
        /// Ignore a recommendation
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <param name="owner">The owner or organization</param>
        /// <param name="repo">The repository name</param>
        /// <param name="recommendationType">The recommendation type</param>
        /// <param name="recommendationDetails">The recommendation details</param>
        /// <returns>True if successful, false otherwise</returns>
        [HttpPost("IgnoreRecommendation")]
        public async Task<ActionResult<bool>> IgnoreRecommendation(string user, string owner, string repo, string recommendationType, string recommendationDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repo) || 
                string.IsNullOrEmpty(recommendationType) || string.IsNullOrEmpty(recommendationDetails))
            {
                return BadRequest("User, owner, repo, recommendationType, and recommendationDetails are required");
            }

            try
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "UseDevelopmentStorage=true";
                IgnoredRecommendationsDA ignoredRecommendationsDA = new IgnoredRecommendationsDA(connectionString);
                await ignoredRecommendationsDA.IgnoreRecommendation(user, owner, repo, recommendationType, recommendationDetails);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Restore/unignore a recommendation
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <param name="owner">The owner or organization</param>
        /// <param name="repo">The repository name</param>
        /// <param name="recommendationType">The recommendation type</param>
        /// <param name="recommendationDetails">The recommendation details</param>
        /// <returns>True if successful, false otherwise</returns>
        [HttpPost("RestoreRecommendation")]
        public async Task<ActionResult<bool>> RestoreRecommendation(string user, string owner, string repo, string recommendationType, string recommendationDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repo) || 
                string.IsNullOrEmpty(recommendationType) || string.IsNullOrEmpty(recommendationDetails))
            {
                return BadRequest("User, owner, repo, recommendationType, and recommendationDetails are required");
            }

            try
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "UseDevelopmentStorage=true";
                IgnoredRecommendationsDA ignoredRecommendationsDA = new IgnoredRecommendationsDA(connectionString);
                await ignoredRecommendationsDA.UnignoreRecommendation(user, owner, repo, recommendationType, recommendationDetails);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get all ignored recommendations for a user, owner, and repository
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <param name="owner">The owner or organization</param>
        /// <param name="repo">The repository name</param>
        /// <returns>List of ignored recommendations</returns>
        [HttpGet("GetIgnoredRecommendations")]
        public async Task<ActionResult<List<IgnoredRecommendation>>> GetIgnoredRecommendations(string user, string owner, string repo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repo))
            {
                return BadRequest("User, owner, and repo are required");
            }

            try
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "UseDevelopmentStorage=true";
                IgnoredRecommendationsDA ignoredRecommendationsDA = new IgnoredRecommendationsDA(connectionString);
                List<IgnoredRecommendation> results = await ignoredRecommendationsDA.GetIgnoredRecommendations(user, owner, repo);
                return results;
            }
            catch
            {
                return new List<IgnoredRecommendation>();
            }
        }

        /// <summary>
        /// Get all ignored recommendations for a user across all repositories
        /// </summary>
        /// <param name="user">The user - often is also the owner, that has access to organizations</param>
        /// <returns>List of ignored recommendations</returns>
        [HttpGet("GetAllIgnoredRecommendations")]
        public async Task<ActionResult<List<IgnoredRecommendation>>> GetAllIgnoredRecommendations(string user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(user))
            {
                return BadRequest("User is required");
            }

            try
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "UseDevelopmentStorage=true";
                IgnoredRecommendationsDA ignoredRecommendationsDA = new IgnoredRecommendationsDA(connectionString);
                List<IgnoredRecommendation> results = await ignoredRecommendationsDA.GetIgnoredRecommendations(user);
                return results;
            }
            catch
            {
                return new List<IgnoredRecommendation>();
            }
        }
    }
}
