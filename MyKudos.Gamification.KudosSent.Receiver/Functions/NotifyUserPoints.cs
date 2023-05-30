using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions;

public class NotifyUserPoints
{

    private IUserPointsService _userPointsService;

    private IScoreMessageSender _scoreMessageSender;

    public NotifyUserPoints(IUserPointsService userPointsService, IScoreMessageSender scoreMessageSender)
    {
        _userPointsService = userPointsService;

        _scoreMessageSender = scoreMessageSender;
    }


    [FunctionName("NotifyUserPoints")]
    public async Task RunAsync([ServiceBusTrigger("NotifyUserPoints", Connection = "KudosServiceBus_ConnectionString")]string myQueueItem, ILogger log)
    {
        try
        {
            var userId = myQueueItem.Replace("\"", "");

            var userPoints = await _userPointsService.GetUserScore(userId);

            if (userPoints != null) {

                await _scoreMessageSender.NotifyProfileScoreUpdated(userPoints);
            
            }



            log.LogInformation($"C# ServiceBus topic trigger function processed message: {myQueueItem}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }
    }
}
