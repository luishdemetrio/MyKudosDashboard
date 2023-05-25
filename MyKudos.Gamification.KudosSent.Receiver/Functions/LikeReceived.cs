using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions;

public class LikeReceived
{
    private string _likeReceiveScore;

    private IGroupScoreRules _groupScoreRules;

    private readonly IScoreMessageSender _scoreQueue;

    public LikeReceived(IConfiguration configuration, IGroupScoreRules groupScoreRules, IScoreMessageSender scoreQueue)
    {
        _likeReceiveScore = configuration["LikeReceivedScore"];

        _groupScoreRules = groupScoreRules;
        _scoreQueue = scoreQueue;
    }


    [FunctionName("GamificationLikeReceived")]
    public async Task Run([ServiceBusTrigger("GamificationLikeReceived", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {
        try
        {

            mySbMsg = mySbMsg.Replace("\"", "");

            var score =
                    new UserScore()
                    {
                        UserId = new Guid(mySbMsg),
                        LikesReceived = 1,
                        Score = int.Parse(_likeReceiveScore)
                    };


            var newScore = await _groupScoreRules.UpdateGroupScoreAsync(score);

            if (newScore != null)
            {
                await _scoreQueue.NotifyProfileScoreUpdated(newScore);
            }
            else
            {
                log.LogError($"Error to read score");
            }

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }
    }
}
