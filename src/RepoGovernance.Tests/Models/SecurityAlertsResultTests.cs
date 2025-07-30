using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SecurityAlertsResultTests
    {
        [TestMethod]
        public void SecurityAlertsResult_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            int expectedCodeScanning = 5;
            int expectedSecretScanning = 3;
            int expectedTotal = 8;

            // Act
            var result = new SecurityAlertsResult(expectedCodeScanning, expectedSecretScanning, expectedTotal);

            // Assert
            Assert.AreEqual(expectedCodeScanning, result.CodeScanningCount);
            Assert.AreEqual(expectedSecretScanning, result.SecretScanningCount);
            Assert.AreEqual(expectedTotal, result.TotalCount);
        }

        [TestMethod]
        public void SecurityAlertsResult_Constructor_HandlesZeroValues()
        {
            // Act
            var result = new SecurityAlertsResult(0, 0, 0);

            // Assert
            Assert.AreEqual(0, result.CodeScanningCount);
            Assert.AreEqual(0, result.SecretScanningCount);
            Assert.AreEqual(0, result.TotalCount);
        }

        [TestMethod]
        public void SecurityAlertsResult_Constructor_HandlesMismatchedTotal()
        {
            // Arrange
            int codeScanning = 2;
            int secretScanning = 3;
            int total = 10; // Intentionally different from sum

            // Act
            var result = new SecurityAlertsResult(codeScanning, secretScanning, total);

            // Assert
            Assert.AreEqual(codeScanning, result.CodeScanningCount);
            Assert.AreEqual(secretScanning, result.SecretScanningCount);
            Assert.AreEqual(total, result.TotalCount);
        }

        [TestMethod]
        public void SecurityAlertsResult_Constructor_HandlesLargeValues()
        {
            // Arrange
            int expectedCodeScanning = 150;
            int expectedSecretScanning = 75;
            int expectedTotal = 225;

            // Act
            var result = new SecurityAlertsResult(expectedCodeScanning, expectedSecretScanning, expectedTotal);

            // Assert
            Assert.AreEqual(expectedCodeScanning, result.CodeScanningCount);
            Assert.AreEqual(expectedSecretScanning, result.SecretScanningCount);
            Assert.AreEqual(expectedTotal, result.TotalCount);
        }

        // Note: The GetFromGitHubAsync static method is not unit tested here as it requires HTTP calls
        // and would be better suited for integration tests. This focuses on the model properties
        // and constructor which can be reliably unit tested.
    }
}