using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver;

public class GamificationLikeReceived
{
    private readonly ILogger<GamificationKudosSent> _logger;
    private readonly IUserScoreService _userScoreService;
    private string _likeReceiveScore;

    public GamificationLikeReceived(ILogger<GamificationKudosSent> log, IConfiguration configuration, IUserScoreService userScoreService)
    {
        _logger = log;
        _userScoreService = userScoreService;
        _likeReceiveScore = configuration["LikeReceivedScore"];
    }


    [FunctionName("GamificationLikeReceived")]
    public async Task Run([ServiceBusTrigger("GamificationLikeReceived", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
    {
        try
        {
            await _userScoreService.SetUserScoreAsync(
                new UserScore()
                {
                    UserId = mySbMsg.Replace("\"", ""),
                    LikesReceived = 1,
                    Score = int.Parse(_likeReceiveScore)
                });

            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");
            throw;
        }
    }
}
