using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Services;

public class GatewayService : IKudosGateway
{

    private readonly string _gatewayServiceUrl;
   
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<GatewayService> _logger;

    public GatewayService(IConfiguration config, IRestClientHelper restClientHelper, ILogger<GatewayService> log)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _restClientHelper = restClientHelper;
        _logger = log;
        
    }


    public async Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber)
    {

        IEnumerable<KudosResponse> kudos = null;

        try
        {
            kudos = await _restClientHelper.GetApiData<IEnumerable<KudosResponse>>($"{_gatewayServiceUrl}kudos/?pageNumber={pageNumber}");
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return kudos;
        
    }


    public async Task<string> SendKudos(KudosRequest kudos)
    {
        string kudosId = string.Empty;

        try
        {
            kudosId = await _restClientHelper.SendApiData<KudosRequest, string>($"{_gatewayServiceUrl}kudos",  HttpMethod.Post,  kudos);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return kudosId;

    }

    public async Task<bool> Like(LikeGateway like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<LikeGateway, bool>($"{_gatewayServiceUrl}likes", HttpMethod.Post, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Like: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UndoLike(LikeGateway like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<LikeGateway, bool>($"{_gatewayServiceUrl}likes", HttpMethod.Delete, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLike: {ex.Message}");
        }

        return result;

    }
    
}
