using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;


namespace MyKudos.Gamification.Receiver.Functions;

public class UndolikeSent
{
    private string _likeSendScore;

    private IGroupScoreRules _groupScoreRules;

    public UndolikeSent(IConfiguration configuration,IGroupScoreRules groupScoreRules)
    {
        _groupScoreRules = groupScoreRules;
        _likeSendScore = configuration["LikeSendScore"];
        
    }

    [FunctionName("GamificationUndolikeSent")]
    public async Task Run([ServiceBusTrigger("GamificationUndolikeSent", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {
        try
        {
            mySbMsg = mySbMsg.Replace("\"", "");

            var score = 
                new UserScore()
                {
                    UserId = mySbMsg,
                    LikesSent = -1,
                    Score = int.Parse(_likeSendScore) * -1
                };

            await _groupScoreRules.UpdateGroupScoreAsync(score);

            log.LogInformation($"C# ServiceBus GamificationUndolikeSent topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }
    }
}
