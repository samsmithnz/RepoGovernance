using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepoGovernance.Core;
using System;
using System.Threading.Tasks;

namespace RepoGovernance.ScheduleFunction
{
    public class ScheduleFunction
    {
        [FunctionName("ScheduleFunction")]
        public static async Task Run([TimerTrigger("0 0 * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            int reposUpdated = 0;
            log.LogInformation($"RepoGovernance Schedule function started execution at: {DateTime.Now}");

            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            await SummaryItemsDA.UpdateSummaryItems(Configuration["AppSettings:GitHubClientId"],
                  Configuration["AppSettings:GitHubClientSecret"],
                  Configuration["AppSettings:StorageConnectionString"],
                  "samsmithnz", 0);

            //Report on the total
            log.LogInformation($"RepoGovernance Schedule function updated {reposUpdated}, completing execution at: {DateTime.Now}");

        }
    }
}
