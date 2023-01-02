using Microsoft.AspNetCore.Mvc;
using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;
using RepoGovernance.Web.Models;
using RepoGovernance.Web.Services;
using System.Diagnostics;

namespace RepoGovernance.Web.Controllers;

public class HomeController : Controller
{
    private readonly SummaryItemsServiceAPIClient _ServiceApiClient;

    public HomeController(SummaryItemsServiceAPIClient ServiceApiClient)
    {
        _ServiceApiClient = ServiceApiClient;
    }

    public async Task<IActionResult> Index()
    {
        List<SummaryItem> summaryItems = await _ServiceApiClient.GetSummaryItems("samsmithnz");
        List<RepoLanguage> repoLanguages = new();
        Dictionary<string, int> repoLanguagesDictonary = new();
        int total = 0;
        foreach (SummaryItem summaryItem in summaryItems)
        {
            foreach (RepoLanguage repoLanguage in summaryItem.RepoLanguages)
            {
                total += repoLanguage.Total;
                if (repoLanguagesDictonary.ContainsKey(repoLanguage.Name))
                {
                    repoLanguagesDictonary[repoLanguage.Name] += repoLanguage.Total;
                }
                else
                {
                    repoLanguagesDictonary.Add(repoLanguage.Name, repoLanguage.Total);
                }
            }
        }     
        SummaryItemsIndex summaryItemsIndex = new()
        {
            SummaryItems = summaryItems,
            SummaryRepoLanguages = repoLanguagesDictonary.OrderByDescending(x => x.Value),
            SummaryRepoLanguagesTotal = total
        };
        return View(summaryItemsIndex);
    }

    //[HttpPost]
    public async Task<IActionResult> UpdateRow(string user, string owner, string repo)
    {
        await _ServiceApiClient.UpdateSummaryItem(user, owner, repo);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateAll()
    {
        List<SummaryItem> summaryItems = await _ServiceApiClient.GetSummaryItems("samsmithnz");
        foreach (SummaryItem summaryItem in summaryItems)
        {
            await _ServiceApiClient.UpdateSummaryItem(summaryItem.User, summaryItem.Owner, summaryItem.Repo);
        }
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
