using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Services;

public class UserScoreService : IUserScoreService
{

    private readonly string _userScoreServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserScoreService> _logger;

    public UserScoreService(IConfiguration config, ILogger<UserScoreService> log, IRestClientHelper clientHelper)
    {
        
        _userScoreServiceUrl = config["userScoreServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<UserScore> GetUserScoreAsync(string pUserId)
    {
        UserScore userScore = null;

        try
        {
            userScore = await _restClientHelper.GetApiData<UserScore>($"{_userScoreServiceUrl}UserScore/?userId={pUserId}");            
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex.Message}");
        }

        return userScore;

    }

    public async Task<bool> SetUserScoreAsync(UserScore userScore)
    {

        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<UserScore, bool>($"{_userScoreServiceUrl}UserScore", Method.Post, userScore);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SetUserScoreAsync: {ex.Message}");
        }

        return result;  
    }
}
