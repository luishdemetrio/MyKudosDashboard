using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Kudos.Notification.Receiver.Interfaces;
using Newtonsoft.Json;

namespace MyKudos.Kudos.Notification.Receiver.functions;

public class SendNotificationCard
{

    private readonly ILogger<SendNotificationCard> _logger;
    private IAgentNotificationService _notificationService;

    public SendNotificationCard(ILogger<SendNotificationCard> log, IAgentNotificationService notificationService)
    {
        _logger = log;
        _notificationService = notificationService;
    }

    [FunctionName("SendNotificationCard")]
    public void Run([ServiceBusTrigger("notifyUserBotTopic", Connection = "KudosServiceBus_ConnectionString")]string myQueueItem, ILogger log)
    {

        try
        {
            try
            {
                var kudos = JsonConvert.DeserializeObject<KudosNotification>(myQueueItem);


                _notificationService.SendNotificationAsync(kudos);

                _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {myQueueItem}");
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
