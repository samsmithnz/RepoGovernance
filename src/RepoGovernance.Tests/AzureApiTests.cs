using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AzureApiTests
    {
        public string? TenantId { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }

        public AzureApiTests()
        {
            //Load the appsettings.json configuration file
            IConfigurationBuilder? builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false)
                 .AddUserSecrets<AzureApiTests>(true);
            IConfigurationRoot configuration = builder.Build();

            TenantId = configuration["AppSettings:AzureTenantId"];
            ClientId = configuration["AppSettings:AzureClientId"];
            ClientSecret = configuration["AppSettings:AzureClientSecret"];
        }

        [TestMethod]
        public async Task GetApplicationsItemTest()
        {
            //Arrange
            Assert.IsNotNull(TenantId);
            Assert.IsNotNull(ClientId);
            Assert.IsNotNull(ClientSecret);
            AzureApi azureApi = new(TenantId, ClientId, ClientSecret);
            AzureDeployment azureDeployment = new();

            //Act
            AzureDeployment result = await azureApi.GetApplications(azureDeployment);

            //Assert
            Assert.IsNotNull(result);
            
        }
    }
}
