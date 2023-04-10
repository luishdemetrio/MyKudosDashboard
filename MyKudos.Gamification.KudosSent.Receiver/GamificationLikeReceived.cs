using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.Gamification.Receiver.Services;

namespace MyKudos.Gamification.Receiver;

public class GamificationLikeReceived
{
    private readonly ILogger<GamificationLikeReceived> _logger;
    private readonly IUserScoreService _userScoreService;
    private string _likeReceiveScore;

    private readonly IScoreQueue _scoreQueue;

    public GamificationLikeReceived(ILogger<GamificationLikeReceived> log, 
                                    IConfiguration configuration, IUserScoreService userScoreService, 
                                    IScoreQueue scoreQueue)
    {
        _logger = log;
        _userScoreService = userScoreService;
        _likeReceiveScore = configuration["LikeReceivedScore"];
        _scoreQueue = scoreQueue;
    }


    [FunctionName("GamificationLikeReceived")]
    public async Task Run([ServiceBusTrigger("GamificationLikeReceived", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
    {
        try
        {
            var result = mySbMsg.Split(",");
            string userId;
            int sign;

            if (result.Length == 2)
            {
                userId = result[0].Replace("\"", "");
                sign = int.Parse(result[1].Replace("\"", ""));

                await _userScoreService.SetUserScoreAsync(
                        new UserScore()
                        {
                            UserId = userId,
                            LikesReceived = 1 * sign,
                            Score = int.Parse(_likeReceiveScore) * sign
                        }
                    );

                var score = await _userScoreService.GetUserScoreAsync(userId);

                if (score != null)
                {

                    await _scoreQueue.NotifyProfileScoreUpdated(score);
                }

            }
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");
            
        }
    }
}
