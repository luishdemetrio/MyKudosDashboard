using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions;

public class MessageReceived
{

    private readonly ILogger<MessageReceived> _logger;
    private readonly IUserScoreService _userScoreService;
    private readonly IScoreQueue _scoreQueue;

    private string _messageReceivedScore;

    public MessageReceived(ILogger<MessageReceived> log,
                                       IConfiguration configuration,
                                       IUserScoreService userScoreService,
                                       IScoreQueue scoreQueue)
    {
        _logger = log;
        _userScoreService = userScoreService;
        _messageReceivedScore = configuration["MessageReceivedScore"];
        _scoreQueue = scoreQueue;
    }


    [FunctionName("MessageReceived")]
    public async Task RunAsync([ServiceBusTrigger("GamificationReplyReceived", Connection = "KudosServiceBus_ConnectionString")]string myQueueItem, ILogger log)
    {
        try
        {
            var userId = myQueueItem.Replace("\"", "");

            await _userScoreService.SetUserScoreAsync(
                new UserScore()
                {
                    UserId = userId,
                    MessagesReceived = 1,
                    Score = int.Parse(_messageReceivedScore)
                });


            var score = await _userScoreService.GetUserScoreAsync(userId);

            if (score != null)
            {

                await _scoreQueue.NotifyProfileScoreUpdated(score);
            }


            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {myQueueItem}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");
            throw;
        }
    }
}
