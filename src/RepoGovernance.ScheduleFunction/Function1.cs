using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace RepoGovernance.ScheduleFunction
{
    public class Function1
    {
        [FunctionName("ScheduleFunction")]
        public void Run([TimerTrigger("0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            int reposUpdated = 0;
            log.LogInformation($"RepoGovernance Schedule function started execution at: {DateTime.Now}");
         
            //Get all repos

            //Loop through repos, updating each one
            
            //HttpResponseMessage response = await client.GetAsync(path);
            //if (response.IsSuccessStatusCode)
            //{
            //    product = await response.Content.ReadAsAsync<Product>();
            //}

            //Report on the total
            log.LogInformation($"RepoGovernance Schedule function updated {reposUpdated}, completing execution at: {DateTime.Now}");

        }
    }
}
