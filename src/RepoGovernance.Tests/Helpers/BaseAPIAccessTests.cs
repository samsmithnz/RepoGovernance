using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class BaseAPIAccessTests
    {
        protected IConfiguration Configuration { get; private set; }

        public BaseAPIAccessTests()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<BaseAPIAccessTests>()
                .Build();
        }

        protected string? GitHubId => Configuration["AppSettings:GitHubClientId"];
        protected string? GitHubSecret => Configuration["AppSettings:GitHubClientSecret"];
        protected string? AzureStorageConnectionString => Configuration["AppSettings:CosmosDBConnectionString"];
        protected string? DevOpsServiceURL => Configuration["AppSettings:DevOpsServiceURL"] ?? "https://devops-prod-eu-service.azurewebsites.net";
        protected string? AzureTenantId => Configuration["AppSettings:AzureTenantId"];
        protected string? AzureClientId => Configuration["AppSettings:AzureClientId"];
        protected string? AzureClientSecret => Configuration["AppSettings:AzureClientSecret"];
    }
}