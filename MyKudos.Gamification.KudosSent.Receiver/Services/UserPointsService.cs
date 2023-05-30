using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using System;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Services;

public class UserPointsService : IUserPointsService
{

    private readonly string _userScoreServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserPointsService> _logger;

    public UserPointsService(IConfiguration config, ILogger<UserPointsService> log, IRestClientHelper clientHelper)
    {
        
        _userScoreServiceUrl = config["UserKudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<UserPointScore> GetUserScore(string pUserId)
    {
        UserPointScore userScore = null;

        try
        {
            userScore = await _restClientHelper.GetApiData<UserPointScore>($"{_userScoreServiceUrl}userpoints/?userId={pUserId}");            
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScore: {ex}");
        }

        return userScore;

    }

  
}
