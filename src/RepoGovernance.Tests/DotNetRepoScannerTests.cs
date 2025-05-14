using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Helpers;

namespace RepoGovernance.Tests.Helpers
{
    [TestClass]
    public class DotNetRepoScannerTests
    {
        [DataTestMethod]
        [DataRow("unknown", "bg-secondary")] // Unknown/gray
        [DataRow("deprecated", "bg-danger")] // Unsupported/End of life/red
        [DataRow("EOL 1.0", "bg-warning")] // Supported, but old/orange
        [DataRow("EOL 2.0", "bg-warning")] // Supported, but old/orange
        [DataRow("supported", "bg-primary")] // Supported/Ok/blue
        [DataRow("in preview", "bg-info")] // Usable, but new - light blue
        [DataRow(null, "bg-secondary")] // Null/Unknown/gray
        [DataRow("random", "bg-secondary")] // Unknown/gray
        public void GetColorFromStatus_ReturnsExpectedColor(string? framework, string expectedColor)
        {
            // Act
            var result = DotNetRepoScanner.GetColorFromStatus(framework);

            // Assert
            Assert.AreEqual(expectedColor, result);
        }
    }
}
