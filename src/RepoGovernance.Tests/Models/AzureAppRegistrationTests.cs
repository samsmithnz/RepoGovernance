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
            AzureAppRegistration appReg = new AzureAppRegistration();

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
            AzureAppRegistration appReg = new AzureAppRegistration();
            DateTimeOffset date1 = DateTimeOffset.Now.AddDays(30);
            DateTimeOffset date2 = DateTimeOffset.Now.AddDays(60);

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
            AzureAppRegistration appReg = new AzureAppRegistration();

            // Act
            DateTimeOffset? result = appReg.ExpirationDate;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDate_SingleDate_ReturnsThatDate()
        {
            // Arrange
            AzureAppRegistration appReg = new AzureAppRegistration();
            DateTimeOffset expectedDate = DateTimeOffset.Now.AddDays(30);
            appReg.ExpirationDates.Add(expectedDate);

            // Act
            DateTimeOffset? result = appReg.ExpirationDate;

            // Assert
            Assert.AreEqual(expectedDate, result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDate_MultipleDates_ReturnsLatestDate()
        {
            // Arrange
            AzureAppRegistration appReg = new AzureAppRegistration();
            DateTimeOffset earlierDate = DateTimeOffset.Now.AddDays(30);
            DateTimeOffset laterDate = DateTimeOffset.Now.AddDays(60);
            appReg.ExpirationDates.Add(earlierDate);
            appReg.ExpirationDates.Add(laterDate);

            // Act
            DateTimeOffset? result = appReg.ExpirationDate;

            // Assert
            Assert.AreEqual(laterDate, result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDate_WithNullValues_ReturnsLatestNonNullDate()
        {
            // Arrange
            AzureAppRegistration appReg = new AzureAppRegistration();
            DateTimeOffset validDate = DateTimeOffset.Now.AddDays(30);
            appReg.ExpirationDates.Add(null);
            appReg.ExpirationDates.Add(validDate);
            appReg.ExpirationDates.Add(null);

            // Act
            DateTimeOffset? result = appReg.ExpirationDate;

            // Assert
            Assert.AreEqual(validDate, result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_NoExpirationDate_ReturnsCorrectMessage()
        {
            // Arrange
            AzureAppRegistration appReg = new AzureAppRegistration();

            // Act
            string result = appReg.ExpirationDateString;

            // Assert
            Assert.AreEqual("No expiration date found", result);
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_FutureDate_ReturnsExpiringMessage()
        {
            // Arrange
            AzureAppRegistration appReg = new AzureAppRegistration();
            DateTimeOffset futureDate = DateTimeOffset.Now.AddDays(30);
            appReg.ExpirationDates.Add(futureDate);

            // Act
            string result = appReg.ExpirationDateString;

            // Assert
            Assert.IsTrue(result.StartsWith("Expiring on"));
            Assert.IsTrue(result.Contains(futureDate.ToString("R")));
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_PastDate_ReturnsExpiredMessage()
        {
            // Arrange
            AzureAppRegistration appReg = new AzureAppRegistration();
            DateTimeOffset pastDate = DateTimeOffset.Now.AddDays(-30);
            appReg.ExpirationDates.Add(pastDate);

            // Act
            string result = appReg.ExpirationDateString;

            // Assert
            Assert.IsTrue(result.StartsWith("Expired on"));
            Assert.IsTrue(result.Contains(pastDate.ToString("R")));
        }

        [TestMethod]
        public void AzureAppRegistration_ExpirationDateString_OnlyNullDates_ReturnsNoExpirationMessage()
        {
            // Arrange
            AzureAppRegistration appReg = new AzureAppRegistration();
            appReg.ExpirationDates.Add(null);
            appReg.ExpirationDates.Add(null);

            // Act
            string result = appReg.ExpirationDateString;

            // Assert
            Assert.AreEqual("No expiration date found", result);
        }
    }
}