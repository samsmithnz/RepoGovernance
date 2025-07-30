using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UserOwnerRepoTests
    {
        [TestMethod]
        public void UserOwnerRepo_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedUser = "testuser";
            string expectedOwner = "testowner";
            string expectedRepo = "testrepo";

            // Act
            var userOwnerRepo = new UserOwnerRepo(expectedUser, expectedOwner, expectedRepo);

            // Assert
            Assert.AreEqual(expectedUser, userOwnerRepo.User);
            Assert.AreEqual(expectedOwner, userOwnerRepo.Owner);
            Assert.AreEqual(expectedRepo, userOwnerRepo.Repo);
        }

        [TestMethod]
        public void UserOwnerRepo_PropertySetters_WorkCorrectly()
        {
            // Arrange
            var userOwnerRepo = new UserOwnerRepo("user1", "owner1", "repo1");

            // Act
            userOwnerRepo.User = "user2";
            userOwnerRepo.Owner = "owner2";
            userOwnerRepo.Repo = "repo2";

            // Assert
            Assert.AreEqual("user2", userOwnerRepo.User);
            Assert.AreEqual("owner2", userOwnerRepo.Owner);
            Assert.AreEqual("repo2", userOwnerRepo.Repo);
        }

        [TestMethod]
        public void UserOwnerRepo_Constructor_HandlesEmptyStrings()
        {
            // Act
            var userOwnerRepo = new UserOwnerRepo("", "", "");

            // Assert
            Assert.AreEqual("", userOwnerRepo.User);
            Assert.AreEqual("", userOwnerRepo.Owner);
            Assert.AreEqual("", userOwnerRepo.Repo);
        }

        [TestMethod]
        public void UserOwnerRepo_Constructor_HandlesDifferentCasing()
        {
            // Arrange
            string user = "TestUser";
            string owner = "TestOwner";
            string repo = "TestRepo";

            // Act
            var userOwnerRepo = new UserOwnerRepo(user, owner, repo);

            // Assert
            Assert.AreEqual("TestUser", userOwnerRepo.User);
            Assert.AreEqual("TestOwner", userOwnerRepo.Owner);
            Assert.AreEqual("TestRepo", userOwnerRepo.Repo);
        }

        [TestMethod]
        public void UserOwnerRepo_Constructor_HandlesSpecialCharacters()
        {
            // Arrange
            string user = "user-123";
            string owner = "owner_456";
            string repo = "repo.name";

            // Act
            var userOwnerRepo = new UserOwnerRepo(user, owner, repo);

            // Assert
            Assert.AreEqual("user-123", userOwnerRepo.User);
            Assert.AreEqual("owner_456", userOwnerRepo.Owner);
            Assert.AreEqual("repo.name", userOwnerRepo.Repo);
        }
    }
}