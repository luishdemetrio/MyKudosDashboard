using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gamification.Receiver.Functions;

public class GamificationKudosReceived
{        
    private string _kudosReceiveScore;

    private IGroupScoreRules _groupScoreRules;

    private readonly IScoreMessageSender _scoreQueue;

    public GamificationKudosReceived(IConfiguration configuration, IGroupScoreRules groupScoreRules,
                                    IScoreMessageSender scoreQueue)
    {
        _kudosReceiveScore = configuration["KudosReceiveScore"];

        _groupScoreRules = groupScoreRules;

        _scoreQueue = scoreQueue;

    }


    [FunctionName("GamificationKudosReceived")]
    public async Task Run([ServiceBusTrigger("GamificationKudosReceived", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {

        try
        {
            var userId = mySbMsg.Replace("\"", "");

            var score = new UserScore()
            {
                UserId = new Guid(userId),
                KudosReceived = 1,
                Score = int.Parse(_kudosReceiveScore)
            };


            var newScore = await _groupScoreRules.UpdateGroupScoreAsync(score);

            if (newScore != null) 
                await _scoreQueue.NotifyProfileScoreUpdated(newScore);

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }

    }

}
