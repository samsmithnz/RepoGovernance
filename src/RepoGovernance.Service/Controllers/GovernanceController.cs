using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core.Models;

namespace RepoGovernance.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernanceController : ControllerBase
    {
        public async Task<List<SummaryItem>> GetSummaryItems()
        {
            return RepoGovernance.Core.APIAccess.DatabaseAccess.GetSummaryItems();
        }
    }
}
