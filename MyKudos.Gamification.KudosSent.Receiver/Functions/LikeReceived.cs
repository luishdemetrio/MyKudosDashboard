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
    private readonly IUserScoreService _userScoreService;
    private string _likeReceiveScore;

    private readonly IScoreMessageSender _scoreQueue;

    public LikeReceived(IConfiguration configuration, IUserScoreService userScoreService,
                        IScoreMessageSender scoreQueue)
    {
        _userScoreService = userScoreService;
        _likeReceiveScore = configuration["LikeReceivedScore"];
        _scoreQueue = scoreQueue;
    }


    [FunctionName("GamificationLikeReceived")]
    public async Task Run([ServiceBusTrigger("GamificationLikeReceived", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {
        try
        {

            mySbMsg = mySbMsg.Replace("\"", "");

            await _userScoreService.SetUserScoreAsync(
                    new UserScore()
                    {
                        UserId = mySbMsg,
                        LikesReceived = 1,
                        Score = int.Parse(_likeReceiveScore)
                    }
                );

            var score = await _userScoreService.GetUserScoreAsync(mySbMsg);

            if (score != null)
            {

                await _scoreQueue.NotifyProfileScoreUpdated(score);
            }


            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }
    }
}
