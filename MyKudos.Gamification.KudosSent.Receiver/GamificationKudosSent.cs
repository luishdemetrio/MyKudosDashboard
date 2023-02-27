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

    public GamificationKudosSent(ILogger<GamificationKudosSent> log, IConfiguration configuration, IUserScoreService userScoreService)
    {
         _logger = log;
        _userScoreService = userScoreService;
        _kudosSendScore =  configuration["KudosSendScore"];
    }

    [FunctionName("gamificationkudossent")]
    public async Task RunAsync([ServiceBusTrigger("gamificationkudossent", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
    {

        try
        {
            await _userScoreService.SetUserScoreAsync(
                new UserScore()
                {
                    UserId = mySbMsg.Replace("\"", ""),
                    KudosSent = 1,
                    Score = int.Parse(_kudosSendScore)
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
