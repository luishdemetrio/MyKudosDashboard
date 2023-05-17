using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions;


public class UndoLikeSentSamePerson
{

    private string _likeSendScore;

    private string _likeReceiveScore;

    private IGroupScoreRules _groupScoreRules;

    private readonly IScoreMessageSender _scoreQueue;

    public UndoLikeSentSamePerson(IConfiguration configuration, IGroupScoreRules groupScoreRules, IScoreMessageSender scoreQueue)
    {

        _likeSendScore = configuration["LikeSendScore"];
        _likeReceiveScore = configuration["LikeReceivedScore"];
        _groupScoreRules = groupScoreRules;
        _scoreQueue = scoreQueue;
    }

    [FunctionName("GamificationUndoLikeSentSamePerson")]
    public async Task Run([ServiceBusTrigger("GamificationUndoLikeSentSamePerson", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg, ILogger log)
    {
        try
        {

            mySbMsg = mySbMsg.Replace("\"", "");

            var score =
                    new UserScore()
                    {
                        Id = new Guid(mySbMsg),
                        LikesReceived = -1,
                        LikesSent = -1,
                        Score = int.Parse(_likeReceiveScore) + int.Parse(_likeReceiveScore) * -1
                    };



            var newScore = await _groupScoreRules.UpdateGroupScoreAsync(score);

            if (newScore != null)
                await _scoreQueue.NotifyProfileScoreSamePersonUpdated(newScore);

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing message: {ex.Message}");

        }
    }
}
