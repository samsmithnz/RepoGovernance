using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernanceController : ControllerBase
    {
        public async Task<List<SummaryItem>> GetSummaryItems(string owner)
        {
            string clientId = "";
            string clientSecret = "";
            return await SummaryItemsController.GetSummaryItems(clientId, clientSecret, owner);
        }
    }
}
