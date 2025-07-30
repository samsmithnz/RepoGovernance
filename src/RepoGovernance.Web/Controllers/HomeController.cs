using Microsoft.AspNetCore.Mvc;
using RepoAutomation.Core.Models;
using RepoGovernance.Core.Models;
using RepoGovernance.Web.Models;
using RepoGovernance.Web.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RepoGovernance.Web.Controllers;

public class HomeController : Controller
{
    private readonly SummaryItemsServiceApiClient _ServiceApiClient;

    public HomeController(SummaryItemsServiceApiClient ServiceApiClient)
    {
        _ServiceApiClient = ServiceApiClient;
    }

    /// <summary>
    /// Validates that a repository name is safe for use in URL fragments.
    /// GitHub repository names can only contain alphanumeric characters, hyphens, underscores, and periods.
    /// </summary>
    /// <param name="repoName">The repository name to validate</param>
    /// <returns>True if the repository name is safe, false otherwise</returns>
    private static bool IsValidRepoName(string? repoName)
    {
        if (string.IsNullOrEmpty(repoName))
            return false;
        
        // GitHub repository names can only contain alphanumeric characters, hyphens, underscores, and periods
        // and cannot start or end with special characters
        return Regex.IsMatch(repoName, @"^[a-zA-Z0-9][a-zA-Z0-9._-]*[a-zA-Z0-9]$|^[a-zA-Z0-9]$");
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

    public async Task<IActionResult> Details(string user, string owner, string repo, bool isContributor = false)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        SummaryItem result = null;
        List<SummaryItem> summaryItems = await _ServiceApiClient.GetSummaryItems(user);
        foreach (SummaryItem summaryItem in summaryItems)
        {
            if (summaryItem.Owner == owner && summaryItem.Repo == repo)
            {
                result = summaryItem;
                break;
            }
        }
        return View(result);
    }


    public async Task<IActionResult> UpdateRow(string user, string owner, string repo, bool isContributor = false)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        await _ServiceApiClient.UpdateSummaryItem(user, owner, repo);

        // Safely pass repo name as query parameter for client-side scrolling
        // Validate repository name to prevent injection attacks
        object routeValues;
        if (IsValidRepoName(repo))
        {
            if (isContributor)
            {
                routeValues = new { isContributor = true, scrollTo = repo };
            }
            else
            {
                routeValues = new { scrollTo = repo };
            }
        }
        else
        {
            if (isContributor)
            {
                routeValues = new { isContributor = true };
            }
            else
            {
                routeValues = new { };
            }
        }

        return RedirectToAction("Index", routeValues);
    }

    public async Task<IActionResult> UpdateAll(bool isContributor = false)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

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
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

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

    public async Task<IActionResult> Config(string user, string owner, string repo, bool isContributor = false)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        SummaryItem? summaryItem = await _ServiceApiClient.GetSummaryItem(user, owner, repo);

        SummaryItemConfig summaryItemConfig = new()
        {
            SummaryItem = summaryItem,
            IsContributor = isContributor
        };

        return View(summaryItemConfig);
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
