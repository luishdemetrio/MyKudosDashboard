using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver;

public class GamificationKudosSent
{
    private readonly ILogger<GamificationKudosSent> _logger;
    private readonly IUserScoreService _userScoreService;
    private string _kudosSendScore;

    private readonly IScoreQueue _scoreQueue;


    public GamificationKudosSent(ILogger<GamificationKudosSent> log, IConfiguration configuration, IUserScoreService userScoreService,
                                 IScoreQueue scoreQueue)
    {
         _logger = log;
        _userScoreService = userScoreService;
        _kudosSendScore =  configuration["KudosSendScore"];
        _scoreQueue = scoreQueue;
    }

    [FunctionName("gamificationkudossent")]
    public async Task RunAsync([ServiceBusTrigger("gamificationkudossent", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
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


            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");
            throw;
        }


      
    }
}
