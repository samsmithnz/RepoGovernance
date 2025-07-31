using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CoverallsCodeCoverageTests
    {
        [TestMethod]
        public void CoverallsCodeCoverage_ParameterlessConstructor_PropertiesAreDefault()
        {
            // Act
            CoverallsCodeCoverage coverage = new CoverallsCodeCoverage();

            // Assert
            Assert.IsNull(coverage.branch);
            Assert.IsNull(coverage.repo_name);
            Assert.IsNull(coverage.badge_url);
            Assert.AreEqual(0.0, coverage.coverage_change);
            Assert.AreEqual(0.0, coverage.covered_percent);
        }

        [TestMethod]
        public void CoverallsCodeCoverage_PropertySetters_WorkCorrectly()
        {
            // Arrange
            CoverallsCodeCoverage coverage = new CoverallsCodeCoverage();

            // Act
            coverage.branch = "main";
            coverage.repo_name = "test-repo";
            coverage.badge_url = "https://example.com/badge.svg";
            coverage.coverage_change = 5.5;
            coverage.covered_percent = 85.7;

            // Assert
            Assert.AreEqual("main", coverage.branch);
            Assert.AreEqual("test-repo", coverage.repo_name);
            Assert.AreEqual("https://example.com/badge.svg", coverage.badge_url);
            Assert.AreEqual(5.5, coverage.coverage_change);
            Assert.AreEqual(85.7, coverage.covered_percent);
        }

        [TestMethod]
        public void CoverallsCodeCoverage_CoverageChangeString_PositiveChange_ReturnsCorrectFormat()
        {
            // Arrange
            CoverallsCodeCoverage coverage = new CoverallsCodeCoverage();
            coverage.coverage_change = 5.7;

            // Act
            string result = coverage.coverage_change_string;

            // Assert
            Assert.AreEqual("+6", result);
        }

        [TestMethod]
        public void CoverallsCodeCoverage_CoverageChangeString_NegativeChange_ReturnsCorrectFormat()
        {
            // Arrange
            CoverallsCodeCoverage coverage = new CoverallsCodeCoverage();
            coverage.coverage_change = -3.2;

            // Act
            string result = coverage.coverage_change_string;

            // Assert
            Assert.AreEqual("--3", result);
        }

        [TestMethod]
        public void CoverallsCodeCoverage_CoverageChangeString_ZeroChange_ReturnsCorrectFormat()
        {
            // Arrange
            CoverallsCodeCoverage coverage = new CoverallsCodeCoverage();
            coverage.coverage_change = 0.0;

            // Act
            string result = coverage.coverage_change_string;

            // Assert
            Assert.AreEqual("+0", result);
        }

        [TestMethod]
        public void CoverallsCodeCoverage_CoverageChangeString_LargePositiveValue_ReturnsCorrectFormat()
        {
            // Arrange
            CoverallsCodeCoverage coverage = new CoverallsCodeCoverage();
            coverage.coverage_change = 15.9;

            // Act
            string result = coverage.coverage_change_string;

            // Assert
            Assert.AreEqual("+16", result);
        }

        [TestMethod]
        public void CoverallsCodeCoverage_CoverageChangeString_LargeNegativeValue_ReturnsCorrectFormat()
        {
            // Arrange
            CoverallsCodeCoverage coverage = new CoverallsCodeCoverage();
            coverage.coverage_change = -25.1;

            // Act
            string result = coverage.coverage_change_string;

            // Assert
            Assert.AreEqual("--25", result);
        }
    }
}