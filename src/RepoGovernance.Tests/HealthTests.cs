using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class HealthTests
    {
        [TestMethod]
        public void GetHealth_ReturnsHealthy()
        {
            // Act
            string result = Health.GetHealth();

            // Assert
            Assert.AreEqual("Healthy", result);
        }
    }
}