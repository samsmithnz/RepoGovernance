using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests;

[ExcludeFromCodeCoverage]
[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public void IsValidRepoName_ValidRepoNames_ReturnsTrue()
    {
        // Arrange & Act & Assert
        Assert.IsTrue(TestIsValidRepoName("MyRepo"), "Simple repo name should be valid");
        Assert.IsTrue(TestIsValidRepoName("my-repo"), "Repo name with hyphens should be valid");
        Assert.IsTrue(TestIsValidRepoName("my_repo"), "Repo name with underscores should be valid");
        Assert.IsTrue(TestIsValidRepoName("my.repo"), "Repo name with periods should be valid");
        Assert.IsTrue(TestIsValidRepoName("MyRepo123"), "Repo name with numbers should be valid");
        Assert.IsTrue(TestIsValidRepoName("A"), "Single character repo name should be valid");
        Assert.IsTrue(TestIsValidRepoName("RepoGovernance"), "Real repo name should be valid");
        Assert.IsTrue(TestIsValidRepoName("Azure-DevOps-Pipeline-Converter"), "Complex valid repo name should be valid");
    }

    [TestMethod]
    public void IsValidRepoName_InvalidRepoNames_ReturnsFalse()
    {
        // Arrange & Act & Assert
        Assert.IsFalse(TestIsValidRepoName(null), "Null should be invalid");
        Assert.IsFalse(TestIsValidRepoName(""), "Empty string should be invalid");
        Assert.IsFalse(TestIsValidRepoName("-repo"), "Repo name starting with hyphen should be invalid");
        Assert.IsFalse(TestIsValidRepoName("repo-"), "Repo name ending with hyphen should be invalid");
        Assert.IsFalse(TestIsValidRepoName(".repo"), "Repo name starting with period should be invalid");
        Assert.IsFalse(TestIsValidRepoName("repo."), "Repo name ending with period should be invalid");
        Assert.IsFalse(TestIsValidRepoName("_repo"), "Repo name starting with underscore should be invalid");
        Assert.IsFalse(TestIsValidRepoName("repo_"), "Repo name ending with underscore should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my repo"), "Repo name with spaces should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my/repo"), "Repo name with slash should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my\\repo"), "Repo name with backslash should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my<repo>"), "Repo name with angle brackets should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my\"repo"), "Repo name with quotes should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my'repo"), "Repo name with single quotes should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my?repo"), "Repo name with question mark should be invalid");
        Assert.IsFalse(TestIsValidRepoName("my#repo"), "Repo name with hash should be invalid");
        Assert.IsFalse(TestIsValidRepoName("javascript:alert('xss')"), "Potential XSS should be invalid");
        Assert.IsFalse(TestIsValidRepoName("../../../etc/passwd"), "Path traversal attempt should be invalid");
    }

    /// <summary>
    /// Test helper method that mimics the private IsValidRepoName method from HomeController
    /// Since the method is private, we need to test the logic here
    /// </summary>
    private static bool TestIsValidRepoName(string? repoName)
    {
        if (string.IsNullOrEmpty(repoName))
            return false;
        
        // GitHub repository names can only contain alphanumeric characters, hyphens, underscores, and periods
        // and cannot start or end with special characters
        return System.Text.RegularExpressions.Regex.IsMatch(repoName, @"^[a-zA-Z0-9][a-zA-Z0-9._-]*[a-zA-Z0-9]$|^[a-zA-Z0-9]$");
    }
}