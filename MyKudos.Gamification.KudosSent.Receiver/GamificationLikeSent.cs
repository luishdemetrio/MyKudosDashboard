using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;


namespace MyKudos.Gamification.Receiver;

public class GamificationLikeSent
{
    private readonly ILogger<GamificationKudosSent> _logger;
    private readonly IUserScoreService _userScoreService;
    private string _likeSendScore;

    public GamificationLikeSent(ILogger<GamificationKudosSent> log, IConfiguration configuration, IUserScoreService userScoreService)
    {
        _logger = log;
        _userScoreService = userScoreService;
        _likeSendScore = configuration["LikeSendScore"];
    }

    [FunctionName("GamificationLikeSent")]
    public async Task Run([ServiceBusTrigger("GamificationLikeSent", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
    {
        try
        {

            var result = mySbMsg.Split(",");
            string userId;
            int sign;

            if (result.Length == 2)
            {
                userId = result[0].Replace("\"", "");
                sign = int.Parse(result[1]);


                await _userScoreService.SetUserScoreAsync(
                    new UserScore()
                    {
                        UserId = userId,
                        LikesSent = 1 * sign,
                        Score = int.Parse(_likeSendScore) * sign
                    });

                
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
