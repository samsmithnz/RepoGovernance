using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RepoGovernance.Tests.APIAccess
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DevOpsMetricServiceApiTests
    {
        [TestMethod]
        public void DevOpsMetricServiceApi_Constructor_ValidUrl_CreatesInstance()
        {
            // Arrange
            string devOpsServiceURL = "https://example.com";

            // Act
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);

            // Assert
            Assert.IsNotNull(api);
        }

        [TestMethod]
        public async Task GetDORASummaryItems_ValidOwnerAndRepo_ReturnsResult()
        {
            // Arrange
            string devOpsServiceURL = "https://example.com";
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            DORASummaryItem? result = await api.GetDORASummaryItems(owner, repo);

            // Assert
            Assert.IsNotNull(result);
            // Since this will likely fail to connect to the test URL, it should return an error item
            Assert.AreEqual(owner, result.Owner);
            Assert.AreEqual(repo, result.Repo);
        }

        [TestMethod]
        public async Task GetDORASummaryItems_InvalidUrl_ReturnsErrorItem()
        {
            // Arrange
            string devOpsServiceURL = "https://invalid-url-that-does-not-exist.com";
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            string owner = "testowner";
            string repo = "testrepo";

            // Act
            DORASummaryItem? result = await api.GetDORASummaryItems(owner, repo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(owner, result.Owner);
            Assert.AreEqual(repo, result.Repo);
            Assert.AreEqual(0, result.DeploymentFrequency);
            Assert.AreEqual(0, result.LeadTimeForChanges);
            Assert.AreEqual(0, result.MeanTimeToRestore);
            Assert.AreEqual(-1, result.ChangeFailureRate);
            Assert.IsTrue(result.LastUpdatedMessage.Contains("Error getting DORA summary item"));
        }

        [TestMethod]
        public async Task GetDORASummaryItems_EmptyOwner_ReturnsResult()
        {
            // Arrange
            string devOpsServiceURL = "https://example.com";
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            string owner = "";
            string repo = "testrepo";

            // Act
            DORASummaryItem? result = await api.GetDORASummaryItems(owner, repo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(owner, result.Owner);
            Assert.AreEqual(repo, result.Repo);
        }

        [TestMethod]
        public async Task GetDORASummaryItems_EmptyRepo_ReturnsResult()
        {
            // Arrange
            string devOpsServiceURL = "https://example.com";
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            string owner = "testowner";
            string repo = "";

            // Act
            DORASummaryItem? result = await api.GetDORASummaryItems(owner, repo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(owner, result.Owner);
            Assert.AreEqual(repo, result.Repo);
        }

        [TestMethod]
        public async Task GetDORASummaryItems_NullOwner_ReturnsResult()
        {
            // Arrange
            string devOpsServiceURL = "https://example.com";
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            string owner = null!;
            string repo = "testrepo";

            // Act
            DORASummaryItem? result = await api.GetDORASummaryItems(owner, repo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(owner, result.Owner);
            Assert.AreEqual(repo, result.Repo);
        }

        [TestMethod]
        public async Task GetDORASummaryItems_NullRepo_ReturnsResult()
        {
            // Arrange
            string devOpsServiceURL = "https://example.com";
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            string owner = "testowner";
            string repo = null!;

            // Act
            DORASummaryItem? result = await api.GetDORASummaryItems(owner, repo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(owner, result.Owner);
            Assert.AreEqual(repo, result.Repo);
        }

        [TestMethod]
        public async Task GetDORASummaryItems_SpecialCharactersInNames_ReturnsResult()
        {
            // Arrange
            string devOpsServiceURL = "https://example.com";
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            string owner = "test-owner.special";
            string repo = "test.repo_name-with-dashes";

            // Act
            DORASummaryItem? result = await api.GetDORASummaryItems(owner, repo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(owner, result.Owner);
            Assert.AreEqual(repo, result.Repo);
        }

        [TestMethod]
        public void DevOpsMetricServiceApi_Constructor_EmptyUrl_CreatesInstance()
        {
            // Arrange
            string devOpsServiceURL = "";

            // Act & Assert
            // This should create an instance but with invalid BaseAddress
            DevOpsMetricServiceApi api = new DevOpsMetricServiceApi(devOpsServiceURL);
            Assert.IsNotNull(api);
        }
    }
}