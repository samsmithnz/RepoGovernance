using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernanceController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public GovernanceController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<string> GetRepos(string owner)
        {
            return SummaryItemsDA.GetRepos(owner);
        }

        [HttpGet("UpdateSummaryItems")]
        public async Task<int> UpdateSummaryItems(string owner, string repo)
        {
            return await SummaryItemsDA.UpdateSummaryItems(
               Configuration["AppSettings:GitHubClientId"],
               Configuration["AppSettings:GitHubClientSecret"],
               Configuration["AppSettings:StorageConnectionString"],
               owner, repo);
        }

        [HttpGet("GetSummaryItems")]
        public List<SummaryItem> GetSummaryItems(string owner)
        {
            return SummaryItemsDA.GetSummaryItems(
                Configuration["AppSettings:StorageConnectionString"],
                owner);
        }
    }
}
