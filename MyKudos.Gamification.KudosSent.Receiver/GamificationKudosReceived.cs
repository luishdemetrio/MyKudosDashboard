using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
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
        private string _kudosReceiveScore;

        public GamificationKudosReceived(ILogger<GamificationKudosReceived> log, IConfiguration configuration, IUserScoreService userScoreService)
        {
            _logger = log;
            _userScoreService = userScoreService;
            _kudosReceiveScore = configuration["KudosReceiveScore"];
        }


        [FunctionName("gamificationKudosReceived")]
        public async Task Run([ServiceBusTrigger("gamificationkudosreceived", "notification", Connection = "KudosServiceBus_ConnectionString")]string mySbMsg)
        {
            try
            {
                await _userScoreService.SetUserScoreAsync(
                    new UserScore()
                    {
                        UserId = mySbMsg.Replace("\"", ""),
                        KudosReceived = 1,
                        Score = int.Parse(_kudosReceiveScore)
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
}
