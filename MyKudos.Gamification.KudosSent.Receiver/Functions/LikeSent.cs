using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;


namespace MyKudos.Gamification.Receiver.Functions;

public class LikeSent
{

    private string _likeSendScore;

    private IGroupScoreRules _groupScoreRules;

    internal LikeSent(IConfiguration configuration, IGroupScoreRules groupScoreRules)
    {
        
        _likeSendScore = configuration["LikeSendScore"];
        _groupScoreRules = groupScoreRules;
    }

    [FunctionName("GamificationLikeSent")]
    public async Task Run([ServiceBusTrigger("GamificationLikeSent", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {
        try
        {
            mySbMsg = mySbMsg.Replace("\"", "");

            var score = 
                new UserScore()
                {
                    UserId = mySbMsg,
                    LikesSent = 1,
                    Score = int.Parse(_likeSendScore)
                };

            await _groupScoreRules.UpdateGroupScoreAsync(score);

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");
            log.LogError($"Error processing message: {ex}");

        }
    }
}
