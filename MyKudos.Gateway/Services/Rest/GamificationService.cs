using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Services.Rest;

public class GamificationService : IGamificationService
{
 

    private readonly string _kudosServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<GamificationService> _logger;

    public GamificationService(IConfiguration config, ILogger<GamificationService> log, IRestClientHelper clientHelper)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<UserScore>> GetTopUserScoresAsync(int top)
    {
        List<UserScore> result = new();

        try
        {
            var contributors = await _restClientHelper.GetApiData<IEnumerable<UserScore>>($"{_kudosServiceUrl}Contributors?top={top}");
            result = contributors.ToList();

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetTopUserScoresAsync: {ex.Message}");
        }

        return result;

        
    }

    public async Task<UserScore> GetUserScoreAsync(string pUserId)
    {
        UserScore result = new();

        try
        {

            result = await _restClientHelper.GetApiData<UserScore>($"{_kudosServiceUrl}UserScore?userid={pUserId}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex.Message}");
        }

        return result;


    }
}
