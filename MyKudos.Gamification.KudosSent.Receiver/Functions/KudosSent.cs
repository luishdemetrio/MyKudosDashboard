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


    public GamificationKudosSent(IConfiguration configuration, IGroupScoreRules groupScoreRules)
    {
        _kudosSendScore = configuration["KudosSendScore"];

        _groupScoreRules = groupScoreRules;
    }

    [FunctionName("GamificationKudosSent")]
    public async Task RunAsync([ServiceBusTrigger("GamificationKudosSent",Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {

        try
        {
            var userId = mySbMsg.Replace("\"", "");

            var score =  new UserScore()
            {
                UserId = userId,
                KudosSent = 1,
                Score = int.Parse(_kudosSendScore)
            };

            await _groupScoreRules.UpdateGroupScoreAsync(score);

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }



    }
}
