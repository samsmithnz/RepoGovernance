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

        [HttpGet("UpdateSummaryItems")]
        public async Task<int> UpdateSummaryItems(string owner)
        {
            return 0;
        }

        [HttpGet("GetSummaryItems")]
        public async Task<List<SummaryItem>> GetSummaryItems(string owner)
        {
            return await SummaryItemsController.GetSummaryItems(
                Configuration["AppSettings:GitHubClientId"],
                Configuration["AppSettings:GitHubClientSecret"],
                Configuration["AppSettings:StorageConnectionString"], owner);
        }
    }
}
