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

    private string _kudosSendScore;

    private IGroupScoreRules _groupScoreRules;

    private readonly IScoreMessageSender _scoreQueue;

    public GamificationKudosSent(IConfiguration configuration, IGroupScoreRules groupScoreRules, IScoreMessageSender scoreQueue)
    {
        _kudosSendScore = configuration["KudosSendScore"];

        _groupScoreRules = groupScoreRules;
        _scoreQueue = scoreQueue;
    }

    [FunctionName("GamificationKudosSent")]
    public async Task RunAsync([ServiceBusTrigger("GamificationKudosSent",Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {

        try
        {
            var userId = mySbMsg.Replace("\"", "");

            var score =  new UserScore()
            {
                UserId = new Guid(userId),
                KudosSent = 1,
                Score = int.Parse(_kudosSendScore)
            };

            var newScore = await _groupScoreRules.UpdateGroupScoreAsync(score);

            if (newScore != null)
                await _scoreQueue.NotifyProfileScoreUpdated(newScore);

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }



    }
}
