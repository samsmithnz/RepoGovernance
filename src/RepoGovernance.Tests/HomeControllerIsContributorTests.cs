using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RepoGovernance.Core.Models;
using RepoGovernance.Web.Controllers;
using RepoGovernance.Web.Models;
using RepoGovernance.Web.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class HomeControllerIsContributorTests
    {
        private HomeController _controller;
        private ISummaryItemsServiceApiClient _mockServiceApiClient;

        [TestInitialize]
        public void Setup()
        {
            _mockServiceApiClient = Substitute.For<ISummaryItemsServiceApiClient>();
            _controller = new HomeController(_mockServiceApiClient);
        }

        [TestMethod]
        public async Task Index_WithIsContributorTrue_SetsViewBagAndModelCorrectly()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.Index(isContributor: true);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(true, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be true");
            
            SummaryItemsIndex model = viewResult.Model as SummaryItemsIndex;
            Assert.IsNotNull(model, "Model should be SummaryItemsIndex");
            Assert.AreEqual(true, model.IsContributor, "Model.IsContributor should be true");
        }

        [TestMethod]
        public async Task Index_WithIsContributorFalse_SetsViewBagAndModelCorrectly()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.Index(isContributor: false);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(false, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be false");
            
            SummaryItemsIndex model = viewResult.Model as SummaryItemsIndex;
            Assert.IsNotNull(model, "Model should be SummaryItemsIndex");
            Assert.AreEqual(false, model.IsContributor, "Model.IsContributor should be false");
        }

        [TestMethod]
        public async Task Details_WithIsContributorTrue_SetsViewBagAndModelCorrectly()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>
            {
                new SummaryItem("testuser", "testowner", "testrepo")
            };
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.Details("testuser", "testowner", "testrepo", isContributor: true);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(true, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be true");
            
            SummaryItemDetails model = viewResult.Model as SummaryItemDetails;
            Assert.IsNotNull(model, "Model should be SummaryItemDetails");
            Assert.AreEqual(true, model.IsContributor, "Model.IsContributor should be true");
            Assert.IsNotNull(model.SummaryItem, "Model.SummaryItem should not be null");
        }

        [TestMethod]
        public async Task Details_WithIsContributorFalse_SetsViewBagAndModelCorrectly()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>
            {
                new SummaryItem("testuser", "testowner", "testrepo")
            };
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.Details("testuser", "testowner", "testrepo", isContributor: false);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(false, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be false");
            
            SummaryItemDetails model = viewResult.Model as SummaryItemDetails;
            Assert.IsNotNull(model, "Model should be SummaryItemDetails");
            Assert.AreEqual(false, model.IsContributor, "Model.IsContributor should be false");
        }

        [TestMethod]
        public async Task TaskList_WithIsContributorTrue_SetsViewBagAndModelCorrectly()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            List<IgnoredRecommendation> mockIgnoredRecommendations = new List<IgnoredRecommendation>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);
            _mockServiceApiClient.GetAllIgnoredRecommendations(Arg.Any<string>()).Returns(mockIgnoredRecommendations);

            // Act
            IActionResult result = await _controller.TaskList(isContributor: true);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(true, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be true");
            
            TaskList model = viewResult.Model as TaskList;
            Assert.IsNotNull(model, "Model should be TaskList");
            Assert.AreEqual(true, model.IsContributor, "Model.IsContributor should be true");
        }

        [TestMethod]
        public async Task TaskList_WithIsContributorFalse_SetsViewBagAndModelCorrectly()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            List<IgnoredRecommendation> mockIgnoredRecommendations = new List<IgnoredRecommendation>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);
            _mockServiceApiClient.GetAllIgnoredRecommendations(Arg.Any<string>()).Returns(mockIgnoredRecommendations);

            // Act
            IActionResult result = await _controller.TaskList(isContributor: false);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(false, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be false");
            
            TaskList model = viewResult.Model as TaskList;
            Assert.IsNotNull(model, "Model should be TaskList");
            Assert.AreEqual(false, model.IsContributor, "Model.IsContributor should be false");
        }

        [TestMethod]
        public void Privacy_WithIsContributorTrue_SetsViewBagCorrectly()
        {
            // Arrange & Act
            IActionResult result = _controller.Privacy(isContributor: true);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(true, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be true");
        }

        [TestMethod]
        public void Privacy_WithIsContributorFalse_SetsViewBagCorrectly()
        {
            // Arrange & Act
            IActionResult result = _controller.Privacy(isContributor: false);

            // Assert
            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Result should be a ViewResult");
            Assert.AreEqual(false, _controller.ViewBag.IsContributor, "ViewBag.IsContributor should be false");
        }

        [TestMethod]
        public async Task UpdateAll_WithIsContributorTrue_RedirectsWithParameter()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.UpdateAll(isContributor: true);

            // Assert
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult, "Result should be a RedirectToActionResult");
            Assert.AreEqual("Index", redirectResult.ActionName, "Should redirect to Index");
            Assert.IsTrue(redirectResult.RouteValues.ContainsKey("isContributor"), "Should contain isContributor parameter");
            Assert.AreEqual(true, redirectResult.RouteValues["isContributor"], "isContributor should be true");
        }

        [TestMethod]
        public async Task UpdateAll_WithIsContributorFalse_RedirectsWithoutParameter()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.UpdateAll(isContributor: false);

            // Assert
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult, "Result should be a RedirectToActionResult");
            Assert.AreEqual("Index", redirectResult.ActionName, "Should redirect to Index");
            Assert.IsFalse(redirectResult.RouteValues?.ContainsKey("isContributor") ?? false, "Should not contain isContributor parameter when false");
        }

        [TestMethod]
        public async Task ApprovePRsForAllRepos_WithIsContributorTrue_RedirectsWithParameter()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.ApprovePRsForAllRepos(isContributor: true);

            // Assert
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult, "Result should be a RedirectToActionResult");
            Assert.AreEqual("Index", redirectResult.ActionName, "Should redirect to Index");
            Assert.IsTrue(redirectResult.RouteValues.ContainsKey("isContributor"), "Should contain isContributor parameter");
            Assert.AreEqual(true, redirectResult.RouteValues["isContributor"], "isContributor should be true");
        }

        [TestMethod]
        public async Task ApprovePRsForAllRepos_WithIsContributorFalse_RedirectsWithoutParameter()
        {
            // Arrange
            List<SummaryItem> mockSummaryItems = new List<SummaryItem>();
            _mockServiceApiClient.GetSummaryItems(Arg.Any<string>()).Returns(mockSummaryItems);

            // Act
            IActionResult result = await _controller.ApprovePRsForAllRepos(isContributor: false);

            // Assert
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult, "Result should be a RedirectToActionResult");
            Assert.AreEqual("Index", redirectResult.ActionName, "Should redirect to Index");
            Assert.IsFalse(redirectResult.RouteValues?.ContainsKey("isContributor") ?? false, "Should not contain isContributor parameter when false");
        }
    }
}