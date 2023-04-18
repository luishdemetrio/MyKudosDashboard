using MyKudos.Communication.Helper.Interfaces;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Services;

public class GamificationGateway : IGamificationGateway
{

    private readonly string _gatewayServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<GamificationGateway> _logger;

    public GamificationGateway(IConfiguration config, ILogger<GamificationGateway> log, IRestClientHelper clientHelper)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<UserScore> GetUserScoreAsync(string pUserId)
    {
        UserScore result = new();

        try
        {

            result = await _restClientHelper.GetApiData<UserScore>($"{_gatewayServiceUrl}gamification?userid={pUserId}");
            
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex.Message}");
        }

        return result;
    }


    public async Task<IEnumerable<TopContributors>> GetTopContributors()
    {
        List<TopContributors> result = new();

        try
        {

            var contributors = await _restClientHelper.GetApiData<IEnumerable<TopContributors>>($"{_gatewayServiceUrl}contributors");
            result = contributors.ToList();

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetTopContributors: {ex.Message}");
        }

        return result;

    }
}
