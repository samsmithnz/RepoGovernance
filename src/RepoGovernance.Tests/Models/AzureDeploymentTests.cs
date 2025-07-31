using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AzureDeploymentTests
    {
        [TestMethod]
        public void AzureDeployment_ParameterlessConstructor_PropertiesAreDefault()
        {
            // Act
            AzureDeployment deployment = new AzureDeployment();

            // Assert
            Assert.IsNotNull(deployment.AppRegistrations);
            Assert.AreEqual(0, deployment.AppRegistrations.Count);
            Assert.IsNull(deployment.DeployedURL);
        }

        [TestMethod]
        public void AzureDeployment_PropertySetters_WorkCorrectly()
        {
            // Arrange
            AzureDeployment deployment = new AzureDeployment();
            AzureAppRegistration appReg = new AzureAppRegistration { Name = "TestApp" };

            // Act
            deployment.DeployedURL = "https://example.com";
            deployment.AppRegistrations.Add(appReg);

            // Assert
            Assert.AreEqual("https://example.com", deployment.DeployedURL);
            Assert.AreEqual(1, deployment.AppRegistrations.Count);
            Assert.AreEqual("TestApp", deployment.AppRegistrations[0].Name);
        }

        [TestMethod]
        public void AzureDeployment_AppRegistrations_CanAddMultiple()
        {
            // Arrange
            AzureDeployment deployment = new AzureDeployment();
            AzureAppRegistration appReg1 = new AzureAppRegistration { Name = "App1" };
            AzureAppRegistration appReg2 = new AzureAppRegistration { Name = "App2" };

            // Act
            deployment.AppRegistrations.Add(appReg1);
            deployment.AppRegistrations.Add(appReg2);

            // Assert
            Assert.AreEqual(2, deployment.AppRegistrations.Count);
            Assert.AreEqual("App1", deployment.AppRegistrations[0].Name);
            Assert.AreEqual("App2", deployment.AppRegistrations[1].Name);
        }

        [TestMethod]
        public void AzureDeployment_DeployedURL_HandleNullValue()
        {
            // Arrange
            AzureDeployment deployment = new AzureDeployment();
            deployment.DeployedURL = "https://example.com";

            // Act
            deployment.DeployedURL = null;

            // Assert
            Assert.IsNull(deployment.DeployedURL);
        }
    }
}