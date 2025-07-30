using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AzureAppRegistrationTests
    {
        [TestMethod]
        public void AzureAppRegistration_ParameterlessConstructor_PropertiesAreDefault()
        {
            // Act
            var appReg = new AzureAppRegistration();

            // Assert
            Assert.IsNull(appReg.Name);
            Assert.IsNotNull(appReg.ExpirationDates);
            Assert.AreEqual(0, appReg.ExpirationDates.Count);
            Assert.IsNull(appReg.ExpirationDate);
        }

        [TestMethod]
        public void AzureAppRegistration_PropertySetters_WorkCorrectly()
        {
            // Arrange
            var appReg = new AzureAppRegistration();
            var date1 = DateTimeOffset.Now.AddDays(30);
            var date2 = DateTimeOffset.Now.AddDays(60);

            // Act
            appReg.Name = "TestApp";
            appReg.ExpirationDates.Add(date1);
            appReg.ExpirationDates.Add(date2);

            // Assert
            Assert.AreEqual("TestApp", appReg.Name);
            Assert.AreEqual(2, appReg.ExpirationDates.Count);
            Assert.AreEqual(date1, appReg.ExpirationDates[0]);
            Assert.AreEqual(date2, appReg.ExpirationDates[1]);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDate_NoExpirationDates_ReturnsNull()
        {
            // Arrange
            var appReg = new AzureAppRegistration();

            // Act
            var result = appReg.ExpirationDate;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDate_SingleDate_ReturnsThatDate()
        {
            // Arrange
            var appReg = new AzureAppRegistration();
            var expectedDate = DateTimeOffset.Now.AddDays(30);
            appReg.ExpirationDates.Add(expectedDate);

            // Act
            var result = appReg.ExpirationDate;

            // Assert
            Assert.AreEqual(expectedDate, result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDate_MultipleDates_ReturnsLatestDate()
        {
            // Arrange
            var appReg = new AzureAppRegistration();
            var earlierDate = DateTimeOffset.Now.AddDays(30);
            var laterDate = DateTimeOffset.Now.AddDays(60);
            appReg.ExpirationDates.Add(earlierDate);
            appReg.ExpirationDates.Add(laterDate);

            // Act
            var result = appReg.ExpirationDate;

            // Assert
            Assert.AreEqual(laterDate, result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDate_WithNullValues_ReturnsLatestNonNullDate()
        {
            // Arrange
            var appReg = new AzureAppRegistration();
            var validDate = DateTimeOffset.Now.AddDays(30);
            appReg.ExpirationDates.Add(null);
            appReg.ExpirationDates.Add(validDate);
            appReg.ExpirationDates.Add(null);

            // Act
            var result = appReg.ExpirationDate;

            // Assert
            Assert.AreEqual(validDate, result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_NoExpirationDate_ReturnsCorrectMessage()
        {
            // Arrange
            var appReg = new AzureAppRegistration();

            // Act
            var result = appReg.ExpirationDateString;

            // Assert
            Assert.AreEqual("No expiration date found", result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_FutureDate_ReturnsExpiringMessage()
        {
            // Arrange
            var appReg = new AzureAppRegistration();
            var futureDate = DateTimeOffset.Now.AddDays(30);
            appReg.ExpirationDates.Add(futureDate);

            // Act
            var result = appReg.ExpirationDateString;

            // Assert
            Assert.IsTrue(result.StartsWith("Expiring on"));
            Assert.IsTrue(result.Contains(futureDate.ToString("R")));
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_PastDate_ReturnsExpiredMessage()
        {
            // Arrange
            var appReg = new AzureAppRegistration();
            var pastDate = DateTimeOffset.Now.AddDays(-30);
            appReg.ExpirationDates.Add(pastDate);

            // Act
            var result = appReg.ExpirationDateString;

            // Assert
            Assert.IsTrue(result.StartsWith("Expired on"));
            Assert.IsTrue(result.Contains(pastDate.ToString("R")));
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_OnlyNullDates_ReturnsNoExpirationMessage()
        {
            // Arrange
            var appReg = new AzureAppRegistration();
            appReg.ExpirationDates.Add(null);
            appReg.ExpirationDates.Add(null);

            // Act
            var result = appReg.ExpirationDateString;

            // Assert
            Assert.AreEqual("No expiration date found", result);
        }
    }
}