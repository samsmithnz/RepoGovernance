using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepoGovernance.Core;
using System;
using System.Threading.Tasks;

namespace RepoGovernance.Function
{
    public class ProcessItemFromQueueFunction
    {
        [FunctionName("ProcessItemFromQueue")]
        public static async Task Run([QueueTrigger("summaryqueue", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log, ExecutionContext context)
        //public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext context)
        {
            //string myQueueItem = "samsmithnz_RepoGovernance";
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            //Split by _, this is the profile, owner, and repo
            string[] parts = myQueueItem.Split('_');
            string profile = "";
            string owner = "";
            string repo = "";
            if (parts.Length == 2)
            {
                profile = parts[0];
                owner = parts[0];
                repo = parts[1];
            }
            else if (parts.Length == 3)
            {
                profile = parts[0];
                owner = parts[1];
                repo = parts[2];
            }
            else
            {
                log.LogError($"Queue had wrong number of parts from {myQueueItem}");
            }

            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<ProcessItemFromQueueFunction>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            //log.LogInformation($"Configurations: ClientId {Configuration["GitHubClientId"]}, ClientSecret {Configuration["GitHubClientSecret"]}, SummaryQueueConnection {Configuration["SummaryQueueConnection"]}");

            int itemsUpdated = await SummaryItemsDA.UpdateSummaryItems(Configuration["GitHubClientId"],
                Configuration["GitHubClientSecret"],
                Configuration["SummaryQueueConnection"],
                profile, owner, repo);
            if (itemsUpdated > 0)
            {
                log.LogInformation($"C# Queue trigger function completed updating {itemsUpdated} items at: {DateTime.Now}");
            }
            else
            {
                log.LogError($"C# Queue trigger function failed updating {itemsUpdated} items at: {DateTime.Now}");
            }

            //return null;
        }
    }
}
