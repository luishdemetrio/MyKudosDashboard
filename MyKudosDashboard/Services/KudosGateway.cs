using Azure.AI.OpenAI;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;

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


    public async Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber,  int pageSize, string userManagerId, int sentOnYear)
    {
        return await GetKudosData($"kudos/?pageNumber={pageNumber}&pageSize={pageSize}",  userManagerId, sentOnYear);
    }

    public async Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber, int pageSize, 
                                                                 string userManagerId,
                                                                 int sentOnYear)
    {
        return await GetKudosData($"kudosfromme/?userid={userId}&pageNumber={pageNumber}&pageSize={pageSize}", 
                                    userManagerId, sentOnYear);
    }

    public async Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber, int pageSize,
                                                               string userManagerId, 
                                                               int sentOnYear)
    {
        return await GetKudosData($"kudosTome/?userid={userId}&pageNumber={pageNumber}&pageSize={pageSize}", userManagerId, sentOnYear);
    }

    private async Task<IEnumerable<KudosResponse>> GetKudosData(string endpoint, string userManagerId, int sentOnYear)
    {
        IEnumerable<KudosResponse> kudos = null;

        try
        {
            kudos = await _restClientHelper.GetApiData<IEnumerable<KudosResponse>>(
                $"{_gatewayServiceUrl}{endpoint}&managerId={userManagerId}&sentOnYear={sentOnYear}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing {nameof(GetKudosData)}: {ex.Message}");
            throw;
        }

        return kudos;
    }


    /// <summary>
    /// Sends Kudos to the Gateway service
    /// </summary>
    /// <param name="kudos"></param>
    /// <returns></returns>
    public async Task<string> SendKudos(SendKudosRequest kudos)
    {
        string kudosId = string.Empty;

        try
        {
            kudosId = await _restClientHelper.SendApiData<SendKudosRequest, string>($"{_gatewayServiceUrl}kudos",  HttpMethod.Post,  kudos);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return kudosId;

    }

    public async Task<bool> Like(SendLikeGateway like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<SendLikeGateway, bool>($"{_gatewayServiceUrl}likes", HttpMethod.Post, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Like: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UndoLike(SendLikeGateway like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<SendLikeGateway, bool>($"{_gatewayServiceUrl}likes", HttpMethod.Delete, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLike: {ex.Message}");
        }

        return result;

    }    

   
    public async Task<bool> UpdateKudos(KudosMessage kudos)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<KudosMessage, bool>($"{_gatewayServiceUrl}kudos", HttpMethod.Put,kudos);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLike: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> DeleteKudos(int kudosId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<int, bool>($"{_gatewayServiceUrl}kudos", HttpMethod.Delete, kudosId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLike: {ex.Message}");
        }

        return result;
    }
}
