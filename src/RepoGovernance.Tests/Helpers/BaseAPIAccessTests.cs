using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace RepoGovernance.Tests.Helpers;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class BaseAPIAccessTests
{
    public string? GitHubId { get; set; }
    public string? GitHubSecret { get; set; }
    public string? AzureStorageConnectionString { get; set; }
    public string? DevOpsServiceURL { get; set; }

    [TestInitialize]
    public void InitializeIntegrationTests()
    {
        //Load the appsettings.json configuration file
        IConfigurationBuilder? builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false)
             .AddUserSecrets<BaseAPIAccessTests>(true);
        IConfigurationRoot configuration = builder.Build();

        GitHubId = configuration["AppSettings:GitHubClientId"];
        GitHubSecret = configuration["AppSettings:GitHubClientSecret"];
        AzureStorageConnectionString = configuration["AppSettings:StorageConnectionString"];
        DevOpsServiceURL = configuration["AppSettings:DevOpsServiceURL"];
    }
}