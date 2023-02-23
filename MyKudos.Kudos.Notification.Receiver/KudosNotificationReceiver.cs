using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Kudos.Notification.Receiver.Interfaces;
using Newtonsoft.Json;

namespace MyKudos.Kudos.Notification.Receiver;

public class KudosNotificationReceiver
{

    private IAgentNotificationService _notificationService;


    public KudosNotificationReceiver(IAgentNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [FunctionName("KudosNotificationReceiver")]
    public void Run([ServiceBusTrigger("kudosqueue", Connection = "ServiceBusConnection")] string message, ILogger log)
    {

        try
        {
            try
            {
                var kudos = JsonConvert.DeserializeObject<KudosNotification>(message);


                _notificationService.SendNotificationAsync(kudos);

                log.LogInformation($"Message processed successfully.");
            }
            catch (Exception ex) { log.LogError($"Error processing message: {ex.Message}"); throw; }

            
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");
            throw;
        }



     


    }
}
