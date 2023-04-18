using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;

namespace MyKudos.Gamification.Receiver.Functions
{
    public class GamificationKudosReceived
    {
        private readonly ILogger<GamificationKudosReceived> _logger;

        private readonly IUserScoreService _userScoreService;

        private readonly IScoreMessageSender _scoreQueue;

        private string _kudosReceiveScore;

        public GamificationKudosReceived(IConfiguration configuration, IUserScoreService userScoreService,
                                         IScoreMessageSender scoreQueue)
        {
            _userScoreService = userScoreService;
            _kudosReceiveScore = configuration["KudosReceiveScore"];
            _scoreQueue = scoreQueue;
        }


        [FunctionName("GamificationKudosReceived")]
        public async Task Run([ServiceBusTrigger("GamificationKudosReceived", Connection = "KudosServiceBus_ConnectionString")] string mySbMsg, ILogger log)
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
}
