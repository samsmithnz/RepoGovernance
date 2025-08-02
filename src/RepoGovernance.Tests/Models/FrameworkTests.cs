using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FrameworkTests
    {
        [TestMethod]
        public void Framework_DefaultConstructor_InitializesCorrectly()
        {
            // Act
            Framework framework = new Framework();

            // Assert
            Assert.IsNull(framework.Name);
            Assert.IsNull(framework.Color);
            Assert.AreEqual(0, framework.Count);
        }

        [TestMethod]
        public void Framework_Properties_CanBeSetAndGet()
        {
            // Arrange
            Framework framework = new Framework();
            string name = "net8.0";
            string color = "green";
            int count = 5;

            // Act
            framework.Name = name;
            framework.Color = color;
            framework.Count = count;

            // Assert
            Assert.AreEqual(name, framework.Name);
            Assert.AreEqual(color, framework.Color);
            Assert.AreEqual(count, framework.Count);
        }

        [TestMethod]
        public void Framework_Properties_AcceptNullValues()
        {
            // Arrange
            Framework framework = new Framework();

            // Act
            framework.Name = null;
            framework.Color = null;
            framework.Count = 0;

            // Assert
            Assert.IsNull(framework.Name);
            Assert.IsNull(framework.Color);
            Assert.AreEqual(0, framework.Count);
        }

        [TestMethod]
        public void Framework_Properties_AcceptEmptyStrings()
        {
            // Arrange
            Framework framework = new Framework();

            // Act
            framework.Name = "";
            framework.Color = "";
            framework.Count = -1; // Test negative count

            // Assert
            Assert.AreEqual("", framework.Name);
            Assert.AreEqual("", framework.Color);
            Assert.AreEqual(-1, framework.Count);
        }

        [TestMethod]
        public void Framework_Count_AcceptsLargeValues()
        {
            // Arrange
            Framework framework = new Framework();
            int largeCount = int.MaxValue;

            // Act
            framework.Count = largeCount;

            // Assert
            Assert.AreEqual(largeCount, framework.Count);
        }

        [TestMethod]
        public void Framework_Name_AcceptsSpecialCharacters()
        {
            // Arrange
            Framework framework = new Framework();
            string specialName = "net-core-3.1_preview!@#$%";

            // Act
            framework.Name = specialName;

            // Assert
            Assert.AreEqual(specialName, framework.Name);
        }

        [TestMethod]
        public void Framework_Color_AcceptsHexColors()
        {
            // Arrange
            Framework framework = new Framework();
            string hexColor = "#FF5733";

            // Act
            framework.Color = hexColor;

            // Assert
            Assert.AreEqual(hexColor, framework.Color);
        }

        [TestMethod]
        public void Framework_Color_AcceptsColorNames()
        {
            // Arrange
            Framework framework = new Framework();
            string colorName = "blue";

            // Act
            framework.Color = colorName;

            // Assert
            Assert.AreEqual(colorName, framework.Color);
        }
    }
}