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

    private string _messageReceivedScore;

    private IGroupScoreRules _groupScoreRules;

    private readonly IScoreMessageSender _scoreQueue;

    public MessageReceived(IConfiguration configuration, IGroupScoreRules groupScoreRules, IScoreMessageSender scoreQueue)
    {
        _messageReceivedScore = configuration["MessageReceivedScore"];

        _groupScoreRules = groupScoreRules;
        _scoreQueue = scoreQueue;
    }


    [FunctionName("MessageReceived")]
    public async Task RunAsync([ServiceBusTrigger("GamificationMessageReceived", Connection = "KudosServiceBus_ConnectionString")]string myQueueItem, ILogger log)
    {
        try
        {
            var userId = myQueueItem.Replace("\"", "");

           var score = 
                new UserScore()
                {
                    Id = new Guid(userId),
                    MessagesReceived = 1,
                    Score = int.Parse(_messageReceivedScore)
                };


            var newScore = await _groupScoreRules.UpdateGroupScoreAsync(score);

            if (newScore != null)
                await _scoreQueue.NotifyProfileScoreUpdated(newScore);

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {myQueueItem}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");
            throw;
        }
    }
}
