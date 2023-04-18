using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.Gamification.Receiver.Services;


namespace MyKudos.Gamification.Receiver.Functions;

public class LikeSent
{
    private readonly IUserScoreService _userScoreService;
    private string _likeSendScore;

    private readonly IScoreMessageSender _scoreQueue;

    public LikeSent(IConfiguration configuration,
                    IUserScoreService userScoreService, IScoreMessageSender scoreQueue)
    {
        _userScoreService = userScoreService;
        _likeSendScore = configuration["LikeSendScore"];
        _scoreQueue = scoreQueue;
    }

    [FunctionName("GamificationLikeSent")]
    public async Task Run([ServiceBusTrigger("GamificationLikeSent", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {
        try
        {
            mySbMsg = mySbMsg.Replace("\"", "");

            await _userScoreService.SetUserScoreAsync(
                new UserScore()
                {
                    UserId = mySbMsg,
                    LikesSent = 1,
                    Score = int.Parse(_likeSendScore)
                });

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
