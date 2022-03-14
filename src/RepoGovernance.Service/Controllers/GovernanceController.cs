﻿using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("Secrets")]
        public string Secrets(string owner)
        {
            return Configuration["AppSettings:GitHubClientId"] + "," + Environment.NewLine +
               Configuration["AppSettings:GitHubClientSecret"] + "," + Environment.NewLine +
               Configuration["AppSettings:StorageConnectionString"];
        }

        [HttpGet("UpdateSummaryItems")]
        public async Task<int> UpdateSummaryItems(string owner)
        {
            return await SummaryItemsDA.UpdateSummaryItems(
               Configuration["AppSettings:GitHubClientId"],
               Configuration["AppSettings:GitHubClientSecret"],
               Configuration["AppSettings:StorageConnectionString"], owner);
        }

        [HttpGet("GetSummaryItems")]
        public List<SummaryItem> GetSummaryItems(string owner)
        {
            return SummaryItemsDA.GetSummaryItems(
                Configuration["AppSettings:StorageConnectionString"], owner);
        }
    }
}
