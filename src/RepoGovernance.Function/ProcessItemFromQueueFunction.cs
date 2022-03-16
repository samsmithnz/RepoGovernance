using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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
        public async Task Run([QueueTrigger("summary-queue", Connection = "SummaryQueueConnection")] string myQueueItem, ILogger log, ExecutionContext context)
        //public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext context)
        {
            //string myQueueItem = "samsmithnz_RepoGovernance";
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            //Split by _, this is the owner and repo
            string[] parts = myQueueItem.Split('_');
            if (parts.Length == 2)
            {
                string owner = parts[0];
                string repo = parts[1];

                IConfigurationRoot Configuration = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets<ProcessItemFromQueueFunction>(optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                log.LogInformation($"Configurations: ClientId {Configuration["GitHubClientId"]}, ClientSecret {Configuration["GitHubClientSecret"]}, SummaryQueueConnection {Configuration["SummaryQueueConnection"]}");

                int itemsUpdated = 0;// await SummaryItemsDA.UpdateSummaryItems(Configuration["GitHubClientId"], Configuration["GitHubClientSecret"], Configuration["SummaryQueueConnection"], owner, repo);
                log.LogInformation($"C# Queue trigger function completed updating {itemsUpdated} items at: {DateTime.Now}");
            }
            else
            {
                log.LogInformation($"Queue had wrong number of parts from {myQueueItem}");
            }
            //return null;
        }
    }
}
