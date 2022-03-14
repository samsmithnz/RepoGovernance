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
            return await SummaryItemsController.UpdateSummaryItems(
               Configuration["AppSettings:GitHubClientId"],
               Configuration["AppSettings:GitHubClientSecret"],
               Configuration["AppSettings:StorageConnectionString"], owner);
        }

        [HttpGet("GetSummaryItems")]
        public List<SummaryItem> GetSummaryItems(string owner)
        {
            return SummaryItemsController.GetSummaryItems(
                Configuration["AppSettings:StorageConnectionString"], owner);
        }
    }
}
