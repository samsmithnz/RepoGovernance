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

    public async Task<IActionResult> Index(bool isContributor = false)
    {
        string currentUser = "samsmithnz";
        List<SummaryItem> summaryItems = await _ServiceApiClient.GetSummaryItems(currentUser);
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
            SummaryRepoLanguages = repoLanguages.OrderByDescending(x => x.Total).ToList(),
            IsContributor = isContributor
        };
        return View(summaryItemsIndex);
    }

    public async Task<IActionResult> UpdateRow(string user, string owner, string repo, bool isContributor = false)
    {
        await _ServiceApiClient.UpdateSummaryItem(user, owner, repo);

        //This is a hack for now - hide controls behind this iscontributor flag, but never show iscontributor=false in query string
        if (isContributor)
        {
            return Redirect(Url.RouteUrl(new { controller = "Home", action = "Index" }) + "?isContributor=true" + "#" + repo);
        }
        else
        {
            return Redirect(Url.RouteUrl(new { controller = "Home", action = "Index" }) + "#" + repo);
        }
    }

    public async Task<IActionResult> UpdateAll(bool isContributor = false)
    {
        string currentUser = "samsmithnz";
        List<SummaryItem> summaryItems = await _ServiceApiClient.GetSummaryItems(currentUser);
        foreach (SummaryItem summaryItem in summaryItems)
        {
            await _ServiceApiClient.UpdateSummaryItem(summaryItem.User, summaryItem.Owner, summaryItem.Repo);
        }

        //This is a hack for now - hide controls behind this iscontributor flag, but never show iscontributor=false in query string
        if (isContributor)
        {
            return RedirectToAction("Index", new { isContributor = true });
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> ApprovePRsForAllRepos(bool isContributor = false)
    {
        string currentUser = "samsmithnz";
        List<SummaryItem> summaryItems = await _ServiceApiClient.GetSummaryItems(currentUser);
        foreach (SummaryItem summaryItem in summaryItems)
        {
            await _ServiceApiClient.ApproveSummaryItemPRs(summaryItem.Owner, summaryItem.Repo, currentUser);
        }

        //This is a hack for now - hide controls behind this iscontributor flag, but never show iscontributor=false in query string
        if (isContributor)
        {
            return RedirectToAction("Index", new { isContributor = true });
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> Config(string owner, string repo)
    {
        SummaryItem? summaryItem = await _ServiceApiClient.GetSummaryItem(owner, repo);

        return View(summaryItem);
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
