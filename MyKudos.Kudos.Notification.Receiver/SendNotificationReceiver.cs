using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MyKudos.Domain.Core.Events;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Kudos.Notification.Receiver.Interfaces;
using Newtonsoft.Json;

namespace MyKudos.Kudos.Notification.Receiver
{
    public class SendNotificationReceiver
    {
        private readonly ILogger<SendNotificationReceiver> _logger;
        private IAgentNotificationService _notificationService;


        public SendNotificationReceiver(ILogger<SendNotificationReceiver> log, IAgentNotificationService notificationService)
        {
            _logger = log;
            _notificationService = notificationService;
        }

        [FunctionName("SendNotificationReceiver")]
        public void Run([ServiceBusTrigger("kudostopic", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
        {
            

            try
            {
                try
                {
                    var kudos = JsonConvert.DeserializeObject<KudosNotification>(mySbMsg);


                    _notificationService.SendNotificationAsync(kudos);

                    _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
                }
                catch (Exception ex) { _logger.LogError($"Error processing message: {ex.Message}"); throw; }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                throw;
            }
        }
    }
}
