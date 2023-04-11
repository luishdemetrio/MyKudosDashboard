using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions;

public class UndolikeReceived
{
    private readonly ILogger<UndolikeReceived> _logger;
    private readonly IUserScoreService _userScoreService;
    private string _likeReceiveScore;

    private readonly IScoreQueue _scoreQueue;

    public UndolikeReceived(ILogger<UndolikeReceived> log,
                                    IConfiguration configuration, IUserScoreService userScoreService,
                                    IScoreQueue scoreQueue)
    {
        _logger = log;
        _userScoreService = userScoreService;
        _likeReceiveScore = configuration["LikeReceivedScore"];
        _scoreQueue = scoreQueue;
    }


    [FunctionName("GamificationUndolikeReceived")]
    public async Task Run([ServiceBusTrigger("GamificationUndolikeReceived", "notification", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg)
    {
        try
        {
            await _userScoreService.SetUserScoreAsync(
                    new UserScore()
                    {
                        UserId = mySbMsg,
                        LikesReceived = -1,
                        Score = int.Parse(_likeReceiveScore) * -1
                    }
                ) ;

            var score = await _userScoreService.GetUserScoreAsync(mySbMsg);

            if (score != null)
            {

                await _scoreQueue.NotifyProfileScoreUpdated(score);
            }


            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");

        }
    }
}