using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Services.Rest;

public class UserPointsService : IUserPointsService
{

    private readonly string _kudosServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserPointsService> _logger;

    public UserPointsService(IConfiguration config, ILogger<UserPointsService> log, IRestClientHelper clientHelper)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<List<UserPoint>> GetTopUserScoresAsync(int top)
    {
        List<UserPoint> result = new();

        try
        {
            var contributors = await _restClientHelper.GetApiData<IEnumerable<UserPoint>>($"{_kudosServiceUrl}TopContributors?top={top}");
            result = contributors.ToList();

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetTopUserScoresAsync: {ex.Message}");
        }

        return result;

    }

    public async Task<UserPointScore> GetUserScoreAsync(string pUserId)
    {
        UserPointScore result = new();

        try
        {

            result = await _restClientHelper.GetApiData<UserPointScore>($"{_kudosServiceUrl}UserPoints?userid={pUserId}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex.Message}");
        }

        return result;
    }
}
