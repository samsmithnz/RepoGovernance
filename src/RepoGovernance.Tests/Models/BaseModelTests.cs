using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace RepoGovernance.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BaseModelTests
    {
        [TestMethod]
        public void BaseModel_ParameterlessConstructor_PropertiesAreDefault()
        {
            // Act
            BaseModel model = new BaseModel();

            // Assert
            Assert.IsNull(model.RawJSON);
        }

        [TestMethod]
        public void BaseModel_PropertySetters_WorkCorrectly()
        {
            // Arrange
            BaseModel model = new BaseModel();
            string testJson = "{\"test\": \"value\"}";

            // Act
            model.RawJSON = testJson;

            // Assert
            Assert.AreEqual(testJson, model.RawJSON);
        }

        [TestMethod]
        public void BaseModel_PropertySetters_HandleNullValue()
        {
            // Arrange
            BaseModel model = new BaseModel();
            model.RawJSON = "initial value";

            // Act
            model.RawJSON = null;

            // Assert
            Assert.IsNull(model.RawJSON);
        }

        [TestMethod]
        public void BaseModel_PropertySetters_HandleEmptyString()
        {
            // Arrange
            BaseModel model = new BaseModel();

            // Act
            model.RawJSON = "";

            // Assert
            Assert.AreEqual("", model.RawJSON);
        }
    }
}