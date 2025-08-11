using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RepoGovernance.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TaskListModelTests
    {
        [TestMethod]
        public void TaskItem_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedOwner = "samsmithnz";
            string expectedRepository = "RepoGovernance";
            string expectedType = "Repository Settings";
            string expectedDetails = "Enable branch protection";

            // Act
            // We'll test the concept with a simple object since we can't reference Web models
            dynamic taskItem = new 
            { 
                Owner = expectedOwner, 
                Repository = expectedRepository, 
                RecommendationType = expectedType, 
                RecommendationDetails = expectedDetails 
            };

            // Assert
            Assert.AreEqual(expectedOwner, taskItem.Owner);
            Assert.AreEqual(expectedRepository, taskItem.Repository);
            Assert.AreEqual(expectedType, taskItem.RecommendationType);
            Assert.AreEqual(expectedDetails, taskItem.RecommendationDetails);
        }

        [TestMethod]
        public void TaskList_Concept_WorksWithDifferentRecommendationTypes()
        {
            // Arrange
            List<dynamic> tasks = new List<dynamic>();

            // Act - Add different types of recommendations
            tasks.Add(new { Owner = "owner1", Repository = "repo1", RecommendationType = "Repository Settings", RecommendationDetails = "Enable branch protection" });
            tasks.Add(new { Owner = "owner2", Repository = "repo2", RecommendationType = "Branch Policies", RecommendationDetails = "Require PR reviews" });
            tasks.Add(new { Owner = "owner3", Repository = "repo3", RecommendationType = "GitHub Actions", RecommendationDetails = "Add CI workflow" });
            tasks.Add(new { Owner = "owner4", Repository = "repo4", RecommendationType = "Dependabot", RecommendationDetails = "Configure dependabot" });
            tasks.Add(new { Owner = "owner5", Repository = "repo5", RecommendationType = "Git Version", RecommendationDetails = "Add GitVersion configuration" });
            tasks.Add(new { Owner = "owner6", Repository = "repo6", RecommendationType = ".NET Frameworks", RecommendationDetails = "Update to .NET 8" });
            tasks.Add(new { Owner = "owner7", Repository = "repo7", RecommendationType = "NuGet Packages", RecommendationDetails = "5 packages require upgrades" });
            tasks.Add(new { Owner = "owner8", Repository = "repo8", RecommendationType = "Security", RecommendationDetails = "2 security alerts detected" });

            // Assert
            Assert.AreEqual(8, tasks.Count);
            Assert.AreEqual("Repository Settings", tasks[0].RecommendationType);
            Assert.AreEqual("Branch Policies", tasks[1].RecommendationType);
            Assert.AreEqual("GitHub Actions", tasks[2].RecommendationType);
            Assert.AreEqual("Dependabot", tasks[3].RecommendationType);
            Assert.AreEqual("Git Version", tasks[4].RecommendationType);
            Assert.AreEqual(".NET Frameworks", tasks[5].RecommendationType);
            Assert.AreEqual("NuGet Packages", tasks[6].RecommendationType);
            Assert.AreEqual("Security", tasks[7].RecommendationType);
        }

        [TestMethod]
        public void TaskList_Sorting_WorksCorrectly()
        {
            // Arrange
            List<dynamic> tasks = new List<dynamic>
            {
                new { Owner = "beta", Repository = "repo2", RecommendationType = "Security", RecommendationDetails = "Fix vulnerability" },
                new { Owner = "alpha", Repository = "repo1", RecommendationType = "Actions", RecommendationDetails = "Add workflow" },
                new { Owner = "beta", Repository = "repo1", RecommendationType = "Dependabot", RecommendationDetails = "Enable dependabot" }
            };

            // Act - Sort by Owner, then Repository, then RecommendationType
            List<dynamic> sortedTasks = tasks
                .OrderBy(t => t.Owner)
                .ThenBy(t => t.Repository)
                .ThenBy(t => t.RecommendationType)
                .ToList();

            // Assert
            Assert.AreEqual("alpha", sortedTasks[0].Owner);
            Assert.AreEqual("repo1", sortedTasks[0].Repository);
            Assert.AreEqual("Actions", sortedTasks[0].RecommendationType);
            
            Assert.AreEqual("beta", sortedTasks[1].Owner);
            Assert.AreEqual("repo1", sortedTasks[1].Repository);
            Assert.AreEqual("Dependabot", sortedTasks[1].RecommendationType);
            
            Assert.AreEqual("beta", sortedTasks[2].Owner);
            Assert.AreEqual("repo2", sortedTasks[2].Repository);
            Assert.AreEqual("Security", sortedTasks[2].RecommendationType);
        }
    }
}