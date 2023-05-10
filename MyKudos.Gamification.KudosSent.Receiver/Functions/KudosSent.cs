using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions;

public class GamificationKudosSent
{
    private readonly IUserScoreService _userScoreService;
    private string _kudosSendScore;

    private readonly IScoreMessageSender _scoreQueue;
    private readonly IConfiguration configuration;

    public GamificationKudosSent(IConfiguration configuration, IUserScoreService userScoreService,
                                 IScoreMessageSender scoreQueue)
    {
        _userScoreService = userScoreService;
        _kudosSendScore = configuration["KudosSendScore"];
        _scoreQueue = scoreQueue;
    }

    [FunctionName("GamificationKudosSent")]
    public async Task RunAsync([ServiceBusTrigger("GamificationKudosSent",Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {

        try
        {
            var userId = mySbMsg.Replace("\"", "");

            await _userScoreService.SetUserScoreAsync(
                new UserScore()
                {
                    UserId = userId,
                    KudosSent = 1,
                    Score = int.Parse(_kudosSendScore)
                });


            var score = await _userScoreService.GetUserScoreAsync(userId);
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
