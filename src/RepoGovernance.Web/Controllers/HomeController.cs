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
                if (repoLanguages.Find(x => x.Name == repoLanguage.Name) == null)
                {
                    repoLanguages.Add(repoLanguage);
                }
            }
            foreach (RepoLanguage repoLanguage in summaryItem.RepoLanguages)
            {
                Debug.WriteLine(summaryItem.Repo + ":" + repoLanguage.Name + ":" + repoLanguage.Total);
            }
        }
        //Update the percent
        foreach (KeyValuePair<string, int> sortedLanguage in repoLanguagesDictonary.OrderByDescending(x => x.Value))
        {
            RepoLanguage? repoLanguage = repoLanguages.Find(x => x.Name == sortedLanguage.Key);
            if (repoLanguage != null)
            {
                repoLanguage.Total = sortedLanguage.Value;
                repoLanguage.Percent = (decimal)repoLanguage.Total / (decimal)total;
            }
        }

        SummaryItemsIndex summaryItemsIndex = new()
        {
            SummaryItems = summaryItems,
            SummaryRepoLanguages = repoLanguages.OrderByDescending(x => x.Total).ToList()
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
