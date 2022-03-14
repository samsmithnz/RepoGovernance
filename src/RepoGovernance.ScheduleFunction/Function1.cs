using System;
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
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
