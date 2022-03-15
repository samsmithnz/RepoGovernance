using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepoGovernance.Core.APIAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoGovernance.Function
{
    public class AddItemsToQueueForProcessingFunction
    {
        [FunctionName("AddItemsToQueueForProcessing")]
        public static async Task Run([TimerTrigger("0 0 * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            string owner = "samsmithnz";
            string queueName = "summary-queue";
            log.LogInformation($"RepoGovernance Schedule function started execution at: {DateTime.Now}");

            //Get the connection string from app settings
            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<AddItemsToQueueForProcessingFunction>(optional: true)
                .AddEnvironmentVariables()
                .Build();
            string connectionString = Configuration["AzureWebJobsStorage"];

            log.LogInformation($"connectionString {connectionString}");

            //Add the repos to the queue for processing
            List<string> repos = DatabaseAccess.GetRepos(owner);
            foreach (string repo in repos)
            {
                //Add the repo to a queue
                string message = owner + "_" + repo;

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new(connectionString, queueName);

                // Create the queue if it doesn't already exist
                queueClient.CreateIfNotExists();
                //Post the message
                if (queueClient.Exists() == true)
                {
                    queueClient.SendMessage(message);
                }
                log.LogInformation($"RepoGovernance added '" + message + "' item to queue, completing execution at: {DateTime.Now}");
            }

            //Report on the total
            log.LogInformation($"{repos.Count} repos added to the queue, finishing execution at: {DateTime.Now} ");
        }
    }
}
