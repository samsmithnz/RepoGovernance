using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace RepoGovernance.Tests
{
    [TestClass]
    public class WorkflowPermissionsTests
    {
        [TestMethod]
        public void WorkflowYaml_ShouldHaveExplicitPermissions_ForMainWorkflow()
        {
            // Arrange
            string workflowPath = Path.Combine("..", "..", "..", "..", "..", ".github", "workflows", "workflow.yml");
            
            // Act
            string yamlContent = File.ReadAllText(workflowPath);
            
            // Assert
            Assert.IsTrue(File.Exists(workflowPath), "Workflow file should exist");
            Assert.IsTrue(yamlContent.Contains("permissions:"), "Workflow should contain explicit permissions");
            Assert.IsTrue(yamlContent.Contains("contents: read"), "Workflow should have contents read permission");
            Assert.IsTrue(yamlContent.Contains("actions: read"), "Workflow should have actions read permission");
            Assert.IsTrue(yamlContent.Contains("id-token: write"), "Deploy jobs should have id-token write permission");
            Assert.IsTrue(yamlContent.Contains("deployments: write"), "Deploy jobs should have deployments write permission");
        }

        [TestMethod]
        public void WorkflowYaml_ShouldHaveExplicitPermissions_ForNightlyProcess()
        {
            // Arrange
            string workflowPath = Path.Combine("..", "..", "..", "..", "..", ".github", "workflows", "nightlyprocess.yml");
            
            // Act
            string yamlContent = File.ReadAllText(workflowPath);
            
            // Assert
            Assert.IsTrue(File.Exists(workflowPath), "Workflow file should exist");
            Assert.IsTrue(yamlContent.Contains("permissions:"), "Workflow should contain explicit permissions");
            Assert.IsTrue(yamlContent.Contains("contents: read"), "Workflow should have contents read permission");
            Assert.IsTrue(yamlContent.Contains("actions: read"), "Workflow should have actions read permission");
        }

        [TestMethod]
        public void CodeQlWorkflow_ShouldAlreadyHavePermissions()
        {
            // Arrange
            string workflowPath = Path.Combine("..", "..", "..", "..", "..", ".github", "workflows", "codeql.yml");
            
            // Act
            string yamlContent = File.ReadAllText(workflowPath);
            
            // Assert
            Assert.IsTrue(File.Exists(workflowPath), "Workflow file should exist");
            Assert.IsTrue(yamlContent.Contains("permissions:"), "CodeQL workflow should already have explicit permissions");
            Assert.IsTrue(yamlContent.Contains("security-events: write"), "CodeQL should have security-events write permission");
            Assert.IsTrue(yamlContent.Contains("packages: read"), "CodeQL should have packages read permission");
            Assert.IsTrue(yamlContent.Contains("contents: read"), "CodeQL should have contents read permission");
        }
    }
}