using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Services;

public class GroupUserScoreService : IGroupUserScoreService
{

    private readonly string _userScoreServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserScoreService> _logger;

    public GroupUserScoreService(IConfiguration config, ILogger<UserScoreService> log, IRestClientHelper clientHelper)
    {

        _userScoreServiceUrl = config["UserKudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }


    public async Task<bool> UpdateGroupScoreAsync(UserScore userScore)
    {
        bool result= false;

        try
        {
            result = await _restClientHelper.SendApiData<UserScore,bool>($"{_userScoreServiceUrl}GroupUserScore", HttpMethod.Post, userScore);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex}");
        }

        return result;
    }
}
