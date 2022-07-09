using Microsoft.AspNetCore.Mvc;
using RepoGovernance.Core.Models;
using RepoGovernance.Web.Models;
using RepoGovernance.Web.Services;
using System.Diagnostics;

namespace RepoGovernance.Web.Controllers;

public class HomeController : Controller
{
    private readonly SummaryItemsServiceAPIClient _ServiceApiClient;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, SummaryItemsServiceAPIClient ServiceApiClient)
    {
        _logger = logger;
        _ServiceApiClient = ServiceApiClient;
    }
    
    public async Task<IActionResult> Index()
    {
        List<SummaryItem> summaryItems = await _ServiceApiClient.GetSummaryItems("samsmithnz");
        return View(summaryItems);
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
