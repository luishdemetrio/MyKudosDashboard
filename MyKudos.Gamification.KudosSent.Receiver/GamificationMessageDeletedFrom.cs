using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MyKudos.Gamification.Receiver
{
    public class GamificationMessageDeletedFrom
    {
        private readonly ILogger<GamificationMessageDeletedFrom> _logger;

        public GamificationMessageDeletedFrom(ILogger<GamificationMessageDeletedFrom> log)
        {
            _logger = log;
        }

        [FunctionName("GamificationMessageDeletedFrom")]
        public void Run([ServiceBusTrigger("GamificationMessageDeletedTo", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
