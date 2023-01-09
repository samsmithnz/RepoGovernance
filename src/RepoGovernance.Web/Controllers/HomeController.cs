using Microsoft.AspNetCore.Mvc;
using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;
using RepoGovernance.Web.Models;
using RepoGovernance.Web.Services;
using System.Diagnostics;

namespace RepoGovernance.Web.Controllers;

public class HomeController : Controller
{
    private readonly SummaryItemsServiceApiClient _ServiceApiClient;

    public HomeController(SummaryItemsServiceApiClient ServiceApiClient)
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
                if (repoLanguage.Name != null)
                {
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
                        repoLanguages.Add(new RepoLanguage
                        {
                            Name = repoLanguage.Name,
                            Total = repoLanguage.Total,
                            Color = repoLanguage.Color,
                            Percent = repoLanguage.Percent
                        });
                    }
                }
            }
        }
        //Update the percent
        foreach (KeyValuePair<string, int> sortedLanguage in repoLanguagesDictonary.OrderByDescending(x => x.Value))
        {
            RepoLanguage? repoLanguage = repoLanguages.Find(x => x.Name == sortedLanguage.Key);
            if (repoLanguage != null)
            {
                repoLanguage.Total = sortedLanguage.Value;
                repoLanguage.Percent = Math.Round((decimal)repoLanguage.Total / (decimal)total * 100M, 1);
            }
        }

        SummaryItemsIndex summaryItemsIndex = new()
        {
            SummaryItems = summaryItems,
            SummaryRepoLanguages = repoLanguages.OrderByDescending(x => x.Total).ToList()
        };
        return View(summaryItemsIndex);
    }

    public async Task<IActionResult> UpdateRow(string user, string owner, string repo)
    {
        await _ServiceApiClient.UpdateSummaryItem(user, owner, repo);
        return Redirect(Url.RouteUrl(new { controller = "Home", action = "Index" }) + "#" + repo);
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
