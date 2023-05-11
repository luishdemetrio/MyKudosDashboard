using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;


namespace MyKudos.Gamification.Receiver.Functions;

public class GamificationKudosReceived
{        
    private string _kudosReceiveScore;

    private IGroupScoreRules _groupScoreRules;


    public GamificationKudosReceived(IConfiguration configuration, IGroupScoreRules groupScoreRules)
    {
        _kudosReceiveScore = configuration["KudosReceiveScore"];

        _groupScoreRules = groupScoreRules;

    }


    [FunctionName("GamificationKudosReceived")]
    public async Task Run([ServiceBusTrigger("GamificationKudosReceived", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
    {

        try
        {
            var userId = mySbMsg.Replace("\"", "");

            var score = new UserScore()
            {
                UserId = userId,
                KudosReceived = 1,
                Score = int.Parse(_kudosReceiveScore)
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
