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
    private readonly IUserScoreService _userScoreService;
    private string _likeSendScore;

    private readonly IScoreMessageSender _scoreQueue;

    public UndolikeSent(IConfiguration configuration,
                        IUserScoreService userScoreService, IScoreMessageSender scoreQueue)
    {
        _userScoreService = userScoreService;
        _likeSendScore = configuration["LikeSendScore"];
        _scoreQueue = scoreQueue;
    }

    [FunctionName("GamificationUndolikeSent")]
    public async Task Run([ServiceBusTrigger("GamificationUndolikeSent", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {
        try
        {
            mySbMsg = mySbMsg.Replace("\"", "");

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

            log.LogInformation($"C# ServiceBus GamificationUndolikeSent topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }
    }
}
