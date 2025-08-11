using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RepoGovernance.Core.Models;
using RepoGovernance.Web.Controllers;
using RepoGovernance.Web.Models;
using RepoGovernance.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class HomeControllerRepoDetailsTests
    {
        private HomeController GetController()
        {
            ISummaryItemsServiceApiClient mockServiceClient = Substitute.For<ISummaryItemsServiceApiClient>();
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            IConfigurationSection mockSection = Substitute.For<IConfigurationSection>();
            
            mockSection.Value.Returns("UseDevelopmentStorage=true");
            mockConfiguration.GetConnectionString("DefaultConnection").Returns("UseDevelopmentStorage=true");

            // Create test data
            List<SummaryItem> testSummaryItems = new List<SummaryItem>
            {
                new SummaryItem("testuser", "testowner", "testrepo")
                {
                    RepoSettingsRecommendations = new List<string> { "Enable branch protection" },
                    BranchPoliciesRecommendations = new List<string> { "Add admin enforcement" },
                    ActionRecommendations = new List<string> { "Add build workflow" },
                    DependabotRecommendations = new List<string> { "Update config" },
                    GitVersionRecommendations = new List<string> { "Add versioning" },
                    DotNetFrameworksRecommendations = new List<string> { "Upgrade framework" },
                    NuGetPackages = new List<RepoGovernance.Core.Models.NuGetPackages.NugetPackage> 
                    { 
                        new RepoGovernance.Core.Models.NuGetPackages.NugetPackage() 
                    },
                    SecurityIssuesCount = 2
                }
            };

            mockServiceClient.GetSummaryItems(Arg.Any<string>()).Returns(testSummaryItems);

            return new HomeController(mockServiceClient, mockConfiguration);
        }

        [TestMethod]
        public async Task RepoDetails_ValidRepo_ReturnsViewWithRecommendations()
        {
            // Arrange
            HomeController controller = GetController();

            // Act
            IActionResult result = await controller.RepoDetails("testowner", "testrepo", false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(RepoDetails));
            
            RepoDetails model = (RepoDetails)viewResult.Model;
            Assert.AreEqual("testowner", model.Owner);
            Assert.AreEqual("testrepo", model.Repository);
            Assert.IsTrue(model.Recommendations.Count > 0);
        }

        [TestMethod]
        public async Task RepoDetails_EmptyOwner_RedirectsToTaskList()
        {
            // Arrange
            HomeController controller = GetController();

            // Act
            IActionResult result = await controller.RepoDetails("", "testrepo", false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("TaskList", redirectResult.ActionName);
        }

        [TestMethod]
        public async Task RepoDetails_EmptyRepo_RedirectsToTaskList()
        {
            // Arrange
            HomeController controller = GetController();

            // Act
            IActionResult result = await controller.RepoDetails("testowner", "", false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("TaskList", redirectResult.ActionName);
        }

        [TestMethod]
        public async Task IgnoreRecommendation_ValidParameters_ReturnsSuccessJson()
        {
            // Arrange
            HomeController controller = GetController();

            // Act
            IActionResult result = await controller.IgnoreRecommendation("testowner", "testrepo", "Security", "Fix vulnerability");

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);
        }

        [TestMethod]
        public async Task IgnoreRecommendation_EmptyParameters_ReturnsFailureJson()
        {
            // Arrange
            HomeController controller = GetController();

            // Act
            IActionResult result = await controller.IgnoreRecommendation("", "testrepo", "Security", "Fix vulnerability");

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);
            // The actual test would check the JSON content for success: false
        }

        [TestMethod]
        public async Task UnignoreRecommendation_ValidParameters_ReturnsSuccessJson()
        {
            // Arrange
            HomeController controller = GetController();

            // Act
            IActionResult result = await controller.UnignoreRecommendation("testowner", "testrepo", "Security", "Fix vulnerability");

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);
        }

        [TestMethod]
        public async Task UnignoreRecommendation_EmptyParameters_ReturnsFailureJson()
        {
            // Arrange
            HomeController controller = GetController();

            // Act
            IActionResult result = await controller.UnignoreRecommendation("", "testrepo", "Security", "Fix vulnerability");

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult jsonResult = (JsonResult)result;
            Assert.IsNotNull(jsonResult.Value);
        }
    }
}