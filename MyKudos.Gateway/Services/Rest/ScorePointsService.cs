using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Services.Rest;

public class ScorePointsService : IScorePointsService
{
    private readonly string _kudosServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<ScorePointsService> _logger;

    public ScorePointsService(IConfiguration config, 
                              ILogger<ScorePointsService> log, 
                              IRestClientHelper clientHelper)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<ScorePoints> GetScore()
    {
        try
        {
            return await _restClientHelper.GetApiData<ScorePoints>(
                                    $"{_kudosServiceUrl}ScorePoints");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetScore: {ex.Message}");

            throw;
        }
    }

    public async Task<bool> UpdateScore(ScorePoints scorePoints)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<ScorePoints, bool>(
                                    $"{_kudosServiceUrl}ScorePoints",
                                    HttpMethod.Put,
                                    scorePoints);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UpdateScore: {ex.Message}");

            throw;
        }

        return result;
    }
}
