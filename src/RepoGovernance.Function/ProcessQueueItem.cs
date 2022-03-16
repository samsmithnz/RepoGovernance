using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace RepoGovernance.Function
{
    public class ProcessQueueItem
    {
        [FunctionName("ProcessQueueItem")]
        public void Run([QueueTrigger("summary-queue", Connection = "SummaryQueueConnection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
