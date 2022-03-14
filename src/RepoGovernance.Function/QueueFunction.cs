using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepoGovernance.Core;

namespace RepoGovernance.Function
{
    public class QueueFunction
    {
        [FunctionName("QueueFunction")]
        public async Task Run([QueueTrigger("summary-queue", Connection = "SummaryQueueConnection")] string myQueueItem, ILogger log, ExecutionContext context)
        {
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
                    .AddEnvironmentVariables()
                    .Build();

                log.LogInformation($"Configurations: ClientId {Configuration["ClientId"]}, ClientSecret {Configuration["ClientSecret"]}, SummaryQueueConnection {Configuration["SummaryQueueConnection"]}");

                int itemsUpdated = await SummaryItemsDA.UpdateSummaryItems(Configuration["ClientId"], Configuration["ClientSecret"], Configuration["SummaryQueueConnection"], owner, repo);
                log.LogInformation($"C# Queue trigger function completed updating {itemsUpdated} items at: {DateTime.Now}");
            }
            else
            {
                log.LogInformation($"Queue had wrong number of parts from {myQueueItem}");
            }
        }
    }
}
