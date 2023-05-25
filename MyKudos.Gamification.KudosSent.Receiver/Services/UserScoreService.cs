using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Services;

public class UserScoreService : IUserScoreService
{

    private readonly string _userScoreServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserScoreService> _logger;

    public UserScoreService(IConfiguration config, ILogger<UserScoreService> log, IRestClientHelper clientHelper)
    {
        
        _userScoreServiceUrl = config["UserKudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<UserScore> GetUserScoreAsync(Guid pUserId)
    {
        UserScore userScore = null;

        try
        {
            userScore = await _restClientHelper.GetApiData<UserScore>($"{_userScoreServiceUrl}UserScore/?userId={pUserId}");            
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex}");
        }

        return userScore;

    }

    public async Task<UserScore> SetUserScoreAsync(UserScore userScore)
    {

        UserScore result = null ;

        try
        {
            result = await _restClientHelper.SendApiData<UserScore, UserScore?>($"{_userScoreServiceUrl}UserScore", HttpMethod.Post, userScore);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SetUserScoreAsync: {ex.Message}");
        }

        return result;  
    }
}
