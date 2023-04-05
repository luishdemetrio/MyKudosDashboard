//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Azure;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using MyKudos.Gamification.Domain.Models;
//using MyKudos.Gamification.Receiver.Interfaces;

//namespace MyKudos.Gamification.Receiver;

//public class GamificationMessageReceived
//{
//    private readonly ILogger<GamificationMessageReceived> _logger;
//    private readonly IUserScoreService _userScoreService;
//    private readonly IScoreQueue _scoreQueue;

//    private string _messageReceivedScore;

//    public GamificationMessageReceived(ILogger<GamificationMessageReceived> log,
//                                       IConfiguration configuration,
//                                       IUserScoreService userScoreService,
//                                       IScoreQueue scoreQueue)
//    {
//        _logger = log;
//        _userScoreService = userScoreService;
//        _messageReceivedScore = configuration["MessageReceivedScore"];
//        _scoreQueue = scoreQueue;
//    }

//    [FunctionName("GamificationMessageReceived")]
//    public async Task Run([ServiceBusTrigger("GamificationLikeReceived", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
//    {
//        try
//        {
//            var userId = mySbMsg.Replace("\"", "");

//            await _userScoreService.SetUserScoreAsync(
//                new UserScore()
//                {
//                    UserId = userId,
//                    MessagesReceived = 1,
//                    Score = int.Parse(_messageReceivedScore)
//                });


//            var score = await _userScoreService.GetUserScoreAsync(userId);

//            if (score != null)
//            {

//                await _scoreQueue.NotifyProfileScoreUpdated(score);
//            }


//            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Error processing message: {ex.Message}");
//            throw;
//        }
//    }
//}