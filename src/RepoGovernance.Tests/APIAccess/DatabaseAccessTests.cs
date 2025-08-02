using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RepoGovernance.Tests.APIAccess
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DatabaseAccessTests
    {
        [TestMethod]
        public void GetRepos_ValidUser_ReturnsExpectedRepositories()
        {
            // Arrange
            string user = "testuser";

            // Act
            List<UserOwnerRepo> result = DatabaseAccess.GetRepos(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            
            // Verify that all entries use the provided user
            Assert.IsTrue(result.All(r => r.User == user));
            
            // Verify specific repositories are included
            Assert.IsTrue(result.Any(r => r.Repo == "AzurePipelinesToGitHubActionsConverter"));
            Assert.IsTrue(result.Any(r => r.Repo == "RepoGovernance"));
            Assert.IsTrue(result.Any(r => r.Repo == "TurnBasedEngine"));
        }

        [TestMethod]
        public void GetRepos_ValidUser_ReturnsExpectedCount()
        {
            // Arrange
            string user = "testuser";

            // Act
            List<UserOwnerRepo> result = DatabaseAccess.GetRepos(user);

            // Assert
            Assert.AreEqual(23, result.Count);
        }

        [TestMethod]
        public void GetRepos_ValidUser_ContainsExpectedOwners()
        {
            // Arrange
            string user = "testuser";

            // Act
            List<UserOwnerRepo> result = DatabaseAccess.GetRepos(user);

            // Assert
            // Most repos should have the user as owner
            Assert.IsTrue(result.Any(r => r.Owner == user));
            
            // Some repos should have different owners
            Assert.IsTrue(result.Any(r => r.Owner == "SamSmithNZ-dotcom"));
            Assert.IsTrue(result.Any(r => r.Owner == "DeveloperMetrics"));
        }

        [TestMethod]
        public void GetRepos_ValidUser_ContainsSpecificRepositories()
        {
            // Arrange
            string user = "testuser";

            // Act
            List<UserOwnerRepo> result = DatabaseAccess.GetRepos(user);

            // Assert
            // Check for some specific repos with different owners
            Assert.IsTrue(result.Any(r => r.Owner == "SamSmithNZ-dotcom" && r.Repo == "SamSmithNZ.com"));
            Assert.IsTrue(result.Any(r => r.Owner == "SamSmithNZ-dotcom" && r.Repo == "MandMCounter"));
            Assert.IsTrue(result.Any(r => r.Owner == "DeveloperMetrics" && r.Repo == "DevOpsMetrics"));
            Assert.IsTrue(result.Any(r => r.Owner == "DeveloperMetrics" && r.Repo == "deployment-frequency"));
            Assert.IsTrue(result.Any(r => r.Owner == "DeveloperMetrics" && r.Repo == "lead-time-for-changes"));
        }

        [TestMethod]
        public void GetRepos_EmptyUser_ReturnsRepositoriesWithEmptyUser()
        {
            // Arrange
            string user = "";

            // Act
            List<UserOwnerRepo> result = DatabaseAccess.GetRepos(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(r => r.User == ""));
        }

        [TestMethod]
        public void GetRepos_NullUser_ReturnsRepositoriesWithNullUser()
        {
            // Arrange
            string user = null!;

            // Act
            List<UserOwnerRepo> result = DatabaseAccess.GetRepos(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(r => r.User == null));
        }

        [TestMethod]
        public void GetRepos_DifferentUsers_ReturnsSameRepositoriesWithDifferentUsers()
        {
            // Arrange
            string user1 = "user1";
            string user2 = "user2";

            // Act
            List<UserOwnerRepo> result1 = DatabaseAccess.GetRepos(user1);
            List<UserOwnerRepo> result2 = DatabaseAccess.GetRepos(user2);

            // Assert
            Assert.AreEqual(result1.Count, result2.Count);
            
            // Check that User properties are set correctly 
            Assert.AreEqual(user1, result1[0].User);
            Assert.AreEqual(user2, result2[0].User);
            
            // Check entries with fixed owners that don't depend on user parameter
            // Find entries with owner "SamSmithNZ-dotcom"
            UserOwnerRepo? samsmithEntry1 = result1.FirstOrDefault(r => r.Owner == "SamSmithNZ-dotcom");
            UserOwnerRepo? samsmithEntry2 = result2.FirstOrDefault(r => r.Owner == "SamSmithNZ-dotcom");
            
            Assert.IsNotNull(samsmithEntry1);
            Assert.IsNotNull(samsmithEntry2);
            Assert.AreEqual(samsmithEntry1.Owner, samsmithEntry2.Owner);
            Assert.AreEqual(samsmithEntry1.Repo, samsmithEntry2.Repo);
            Assert.AreEqual(user1, samsmithEntry1.User);
            Assert.AreEqual(user2, samsmithEntry2.User);
        }
    }
}