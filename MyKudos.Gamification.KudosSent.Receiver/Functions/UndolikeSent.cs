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
    private readonly ILogger<UndolikeSent> _logger;
    private readonly IUserScoreService _userScoreService;
    private string _likeSendScore;

    private readonly IScoreQueue _scoreQueue;

    public UndolikeSent(ILogger<UndolikeSent> log, IConfiguration configuration,
                                IUserScoreService userScoreService, IScoreQueue scoreQueue)
    {
        _logger = log;
        _userScoreService = userScoreService;
        _likeSendScore = configuration["LikeSendScore"];
        _scoreQueue = scoreQueue;
    }

    [FunctionName("GamificationUndolikeSent")]
    public async Task Run([ServiceBusTrigger("GamificationUndolikeSent", "notification", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg)
    {
        try
        {

            await _userScoreService.SetUserScoreAsync(
                new UserScore()
                {
                    UserId = mySbMsg,
                    LikesSent = -1,
                    Score = int.Parse(_likeSendScore) * -1
                });

            var score = await _userScoreService.GetUserScoreAsync(mySbMsg);

            if (score != null)
            {

                await _scoreQueue.NotifyProfileScoreUpdated(score);
            }

            _logger.LogInformation($"C# ServiceBus GamificationUndolikeSent topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");

        }
    }
}
