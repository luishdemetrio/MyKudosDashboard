using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver
{
    public class GamificationKudosReceived
    {
        private readonly ILogger<GamificationKudosReceived> _logger;

        private readonly IUserScoreService _userScoreService;

        private readonly IScoreQueue _scoreQueue;

        private string _kudosReceiveScore;

        public GamificationKudosReceived(ILogger<GamificationKudosReceived> log, IConfiguration configuration, IUserScoreService userScoreService,
                                         IScoreQueue scoreQueue)
        {
            _logger = log;
            _userScoreService = userScoreService;
            _kudosReceiveScore = configuration["KudosReceiveScore"];
            _scoreQueue = scoreQueue;
        }


        [FunctionName("GamificationKudosReceived")]
        public async Task Run([ServiceBusTrigger("GamificationKudosReceived", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
        {
            
            try
            {
                var userId = mySbMsg.Replace("\"", "");

                await _userScoreService.SetUserScoreAsync(
                    new UserScore()
                    {
                        UserId = userId,
                        KudosReceived = 1,
                        Score = int.Parse(_kudosReceiveScore)
                    });


                var score = await _userScoreService.GetUserScoreAsync(userId);

                if (score != null) {

                    await _scoreQueue.NotifyProfileScoreUpdated(score);
                }


                _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                
            }

        }
    }
}
