using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces.Aggregator;

namespace MyKudosDashboard.Services;

public class ScorePointsAggregator : IScorePointsAggregator
{

    private readonly string _serviceUrl;
    private IRestClientHelper _restClientHelper;
    private readonly ILogger<ScorePointsAggregator> _logger;

    public ScorePointsAggregator(IConfiguration config, IRestClientHelper clientHelper,
                            ILogger<ScorePointsAggregator> log)
    {
        _serviceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<Points> GetPoints()
    {
        try
        {
            return await _restClientHelper.GetApiData<Points>($"{_serviceUrl}ScorePoints");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing GetUserScoreAsync: {ex.Message}");

            throw;
        }
    }

    public async Task<bool> UpdateScore(Points points)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Points, bool>(
                                    $"{_serviceUrl}ScorePoints",
                                    HttpMethod.Put,
                                    points);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UpdateScore: {ex.Message}");

            throw;
        }

        return result;
    }
}