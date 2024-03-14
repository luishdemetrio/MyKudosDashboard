using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;

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

    public async Task<UserPointScore> GetUserScoreAsync(string pUserId, int sentOnYear, bool justMyTeam = false)
    {
        UserPointScore result = new();

        try
        {

            result = await _restClientHelper.GetApiData<UserPointScore>(
                            $"{_gatewayServiceUrl}UserPoints/GetUserPoints/{pUserId},{justMyTeam},{sentOnYear}");
            
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserScoreAsync: {ex.Message}");
        }

        return result;
    }


    public async Task<IEnumerable<TopContributors>> GetTopContributors(int sentOnYear, Guid? managerId)
    {
        List<TopContributors> result = new();

        try
        {

            var contributors = await _restClientHelper.GetApiData<IEnumerable<TopContributors>>(
                            $"{_gatewayServiceUrl}contributors/?managerId={managerId}&sentOnYear={sentOnYear}");
            result = contributors.ToList();

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetTopContributors: {ex.Message}");
        }

        return result;

    }
}
