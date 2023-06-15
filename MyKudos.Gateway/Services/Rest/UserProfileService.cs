using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Services.Rest;

public class UserProfileService : IUserProfileService
{

    private readonly string _kudosServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserProfileService> _logger;

    public UserProfileService(IConfiguration config, ILogger<UserProfileService> log, IRestClientHelper clientHelper)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<List<UserProfile>> GetUsers(string name)
    {
        List<UserProfile> result = new();

        try
        {
            result = await _restClientHelper.GetApiData<List<UserProfile>>($"{_kudosServiceUrl}userprofile?name={name}");
            

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetTopUserScoresAsync: {ex.Message}");
        }

        return result;

    }


}
