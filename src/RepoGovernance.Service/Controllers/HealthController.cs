﻿using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core;

namespace RepoGovernance.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        public string Get()
        {
            return Health.GetHealth();
        }
    }
}
