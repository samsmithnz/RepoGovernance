using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DotNetRepoScannerTests
    {
        [TestMethod]
        public void GetColorFromStatus_Unknown_ReturnsSecondary()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("unknown");

            // Assert
            Assert.AreEqual("bg-secondary", result);
        }

        [TestMethod]
        public void GetColorFromStatus_Deprecated_ReturnsDanger()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("deprecated");

            // Assert
            Assert.AreEqual("bg-danger", result);
        }

        [TestMethod]
        public void GetColorFromStatus_Supported_ReturnsPrimary()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("supported");

            // Assert
            Assert.AreEqual("bg-primary", result);
        }

        [TestMethod]
        public void GetColorFromStatus_InPreview_ReturnsInfo()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("in preview");

            // Assert
            Assert.AreEqual("bg-info", result);
        }

        [TestMethod]
        public void GetColorFromStatus_EOLPrefix_ReturnsWarning()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("EOL 2024-01-01");

            // Assert
            Assert.AreEqual("bg-warning", result);
        }

        [TestMethod]
        public void GetColorFromStatus_EOLPrefix_AnotherExample_ReturnsWarning()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("EOL next year");

            // Assert
            Assert.AreEqual("bg-warning", result);
        }

        [TestMethod]
        public void GetColorFromStatus_Null_ReturnsSecondary()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus(null);

            // Assert
            Assert.AreEqual("bg-secondary", result);
        }

        [TestMethod]
        public void GetColorFromStatus_EmptyString_ReturnsSecondary()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("");

            // Assert
            Assert.AreEqual("bg-secondary", result);
        }

        [TestMethod]
        public void GetColorFromStatus_RandomString_ReturnsSecondary()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("random-status");

            // Assert
            Assert.AreEqual("bg-secondary", result);
        }

        [TestMethod]
        public void GetColorFromStatus_CaseSensitive_ReturnsSecondary()
        {
            // Test that the method is case-sensitive
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("SUPPORTED");

            // Assert
            Assert.AreEqual("bg-secondary", result);
        }

        [TestMethod]
        public void GetColorFromStatus_PartialEOLMatch_ReturnsSecondary()
        {
            // Test that EOL must be at the start
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("not EOL");

            // Assert
            Assert.AreEqual("bg-secondary", result);
        }

        [TestMethod]
        public void GetColorFromStatus_WhitespaceString_ReturnsSecondary()
        {
            // Act
            string? result = DotNetRepoScanner.GetColorFromStatus("   ");

            // Assert
            Assert.AreEqual("bg-secondary", result);
        }
    }
}