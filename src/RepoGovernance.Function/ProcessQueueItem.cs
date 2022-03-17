//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;

//namespace RepoGovernance.Function
//{
//    public class ProcessQueueItem
//    {
//        [FunctionName("ProcessQueueItem")]
//        public void Run([QueueTrigger("summaryqueue", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
//        {
//            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
//        }
//    }
//}
