using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Gateway.Services.Rest;

public class UserProfileService : IUserProfileService
{

    private readonly string _kudosServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserProfileService> _logger;
    private readonly object userid;

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

    public async Task<string> GetUserPhoto(Guid userid)
    {
        string result = string.Empty;

        try
        {
            result = await _restClientHelper.GetApiData<string>($"{_kudosServiceUrl}photo/?userid={userid}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserPhoto: {ex.Message}");
        }

        return result;
    }

    public async Task<List<UserProfile>> GetManagers(Guid[] ids)
    {
        List<UserProfile> result = new();

        try
        {
            result = await _restClientHelper.SendApiData< Guid[], List<UserProfile>>($"{_kudosServiceUrl}manager", HttpMethod.Post, ids);


        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetTopUserScoresAsync: {ex.Message}");
        }

        return result;
    }

    public async Task<UserProfile?> GetUser(Guid userId)
    {
        UserProfile result = null;

        try
        {
            result = await _restClientHelper.GetApiData<UserProfile>($"{_kudosServiceUrl}userinfo/?userid={userId}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserPhoto: {ex.Message}");
        }

        return result;
    }
}
