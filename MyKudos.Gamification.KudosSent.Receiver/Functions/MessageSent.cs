using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions;

public class MessageSent
{

    private string _messageSentScore;

    private IGroupScoreRules _groupScoreRules;

    public MessageSent(IConfiguration configuration, IGroupScoreRules groupScoreRules) 
    { 

        _messageSentScore = configuration["MessageSentScore"];
        _groupScoreRules = groupScoreRules;
    }

    [FunctionName("MessageSent")]
    public async Task RunAsync([ServiceBusTrigger("GamificationMessageSent", Connection = "KudosServiceBus_ConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            var userId = myQueueItem.Replace("\"", "");

            var score = 
                new UserScore()
                {
                    UserId = userId,
                    MessagesSent = 1,
                    Score = int.Parse(_messageSentScore)
                };


            await _groupScoreRules.UpdateGroupScoreAsync(score);


            log.LogInformation($"C# ServiceBus topic trigger function processed message: {myQueueItem}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");
            throw;
        }
    }
}
