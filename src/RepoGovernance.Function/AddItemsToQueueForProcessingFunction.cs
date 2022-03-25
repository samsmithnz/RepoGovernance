using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepoGovernance.Core.APIAccess;
using RepoGovernance.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepoGovernance.Function
{
    public class AddItemsToQueueForProcessingFunction
    {
        [FunctionName("AddItemsToQueueForProcessing")]
        // 0 0 * * * //Every 24 hours
        // */5 * * * * //Every 5 minutes
        // 0 * * * * //Every 60 mins
        //public static void Run([TimerTrigger("0 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext context)
        {
            string profile = "samsmithnz";
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
            List<ProfileOwnerRepo> repos = DatabaseAccess.GetRepos(profile);
            int visibilityMinuteDelay = 0;
            foreach (ProfileOwnerRepo repo in repos)
            {
                //Add the repo to a queue, with the format [profile]_[owner]_[repo]
                string message = repo.Profile + "_" + repo.Owner + "_" + repo.Repo;

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
                    queueClient.SendMessage(messageText: message,
                        visibilityTimeout: new TimeSpan(0, visibilityMinuteDelay, 0),
                        timeToLive: new TimeSpan(12, 0, 0));
                }
                log.LogInformation($"AddItemsToQueueForProcessing added '" + message + "' item to queue, completing execution at: {DateTime.Now}");

                //Note that because the GitHub search API has a 30 request per minute delay, we will add each item to the queue here with a delay
                visibilityMinuteDelay++;
            }

            //Report on the total
            log.LogInformation($"{repos.Count} repos added to the queue, finishing execution at: {DateTime.Now} ");
            return null;
        }
    }
}
