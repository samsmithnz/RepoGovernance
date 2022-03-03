using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using RepoGovernance.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoGovernance.Tests;

[TestClass]
public class DatabaseAccessTests : BaseAPIAccessTests
{
    [TestMethod]
    public void GetSummaryItems()
    {
        //Arrange
        string owner = "samsmithnz";

        //Act
        List<SummaryItem> summaryItems = DatabaseAccess.GetSummaryItems(owner);

        //Assert
        Assert.IsNotNull(summaryItems);
        Assert.IsTrue(summaryItems.Count > 0);
        Assert.AreEqual("AzurePipelinesToActions", summaryItems[0].Repo);
        Assert.IsNotNull(summaryItems[0].Actions);
        Assert.AreEqual(0, summaryItems[0].Actions.Count);
    }

}