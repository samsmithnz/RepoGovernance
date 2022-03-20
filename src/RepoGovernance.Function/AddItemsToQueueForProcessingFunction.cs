using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepoGovernance.Core.APIAccess;
using System;
using System.Collections.Generic;

namespace RepoGovernance.Function
{
    public class AddItemsToQueueForProcessingFunction
    {
        [FunctionName("AddItemsToQueueForProcessing")]
        // 0 0 * * * //Every 24 hours
        // */5 * * * * //Every 5 minutes
        // 0 * * * * //Every 60 mins
        public static void Run([TimerTrigger("0 0 * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            string owner = "samsmithnz";
            string queueName = "summaryqueue";
            log.LogInformation($"AddItemsToQueueForProcessing function started execution at: {DateTime.Now}");

            //Get the connection string from app settings
            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<AddItemsToQueueForProcessingFunction>(optional: true)
                .AddEnvironmentVariables()
                .Build();
            string connectionString = Configuration["AzureWebJobsStorage"];

            //log.LogInformation($"connectionString {connectionString}");

            //Add the repos to the queue for processing
            List<(string,string)> repos = DatabaseAccess.GetRepos(owner);
            foreach ((string,string) repo in repos)
            {
                //Add the repo to a queue
                string message = repo.Item1 + "_" + repo.Item2;

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClientOptions options = new()
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                };
                QueueClient queueClient = new(connectionString, queueName, options);

                // Create the queue if it doesn't already exist
                queueClient.CreateIfNotExists();
                //Post the message
                if (queueClient.Exists() == true)
                {
                    queueClient.SendMessage(messageText: message, timeToLive: new TimeSpan(12, 0, 0));
                }
                log.LogInformation($"AddItemsToQueueForProcessing added '" + message + "' item to queue, completing execution at: {DateTime.Now}");
            }

            //Report on the total
            log.LogInformation($"{repos.Count} repos added to the queue, finishing execution at: {DateTime.Now} ");
        }
    }
}
