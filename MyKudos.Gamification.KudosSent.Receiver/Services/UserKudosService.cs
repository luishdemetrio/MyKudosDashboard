using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.Kudos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Services;

internal class UserKudosService : IUserKudosService
{

    private readonly string _userKudosService;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserScoreService> _logger;


    public UserKudosService(IConfiguration config, ILogger<UserScoreService> log, IRestClientHelper clientHelper)
    {

        _userKudosService = config["UserKudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(string pUserId)
    {
        IEnumerable<KudosGroupedByValue> result = null ;

        try
        {
            result = await _restClientHelper.GetApiData<IEnumerable<KudosGroupedByValue>>($"{_userKudosService}UserKudos/?userId={pUserId}");
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex}");
        }

        return result;
    }
}
