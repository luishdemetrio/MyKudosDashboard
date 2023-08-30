using Dapr.Client;
using MyKudos.Communication.Helper.Interfaces;
using SuperKudos.Aggregator.Domain.Models;
using MyKudosDashboard.Interfaces;
using SuperKudos.Aggregator.Protos;
using Grpc.Net.Client;

namespace MyKudosDashboard.Services;

public class GatewayService : IKudosGateway
{

    private readonly string _gatewayServiceUrl;
   
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<GatewayService> _logger;

    private readonly DaprClient _daprClient;

    public GatewayService(IConfiguration config, IRestClientHelper restClientHelper, ILogger<GatewayService> log,
                            DaprClient daprClient)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _restClientHelper = restClientHelper;
        _logger = log;
        _daprClient = daprClient;
        
    }


    public async Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber)
    {

        List<KudosResponse> kudos = null;

        try
        {

            using var channel = GrpcChannel.ForAddress(_gatewayServiceUrl);

            var client = new KudosService.KudosServiceClient(channel);

            var reply = await client.GetKudosAsync(new GetKudosRequest() { PageNumber = pageNumber });

            //var request = new GetKudosRequest { PageNumber = 1 };
            //var response = await _daprClient.InvokeMethodGrpcAsync<GetKudosRequest, PaginatedGetKudosResponse>("kudosaggregator", "GetKudos", request);

            if (reply != null)
            {
                kudos = new List<KudosResponse>();

                foreach (var response in reply.Data)
                {
                    var kr = new KudosResponse()
                    {
                        Id = response.Id,
                        From = new SuperKudos.Aggregator.Domain.Models.Person()
                        {
                            Id = new Guid(response.From.Id),
                            GivenName = response.From.GivenName,
                            Name = response.From.Name,
                            Photo = response.From.Photo
                        },
                        Message = response.Message,
                        SendOn = response.SendOn.ToDateTime(),
                        Title = response.Title,
                        Comments = response.Comments.ToList()

                    };

                    foreach(var like in response.Likes)
                    {
                        kr.Likes.Add(new SuperKudos.Aggregator.Domain.Models.Person()
                        {
                            Id = new Guid(like.Id),
                            GivenName = like.GivenName,
                            Name = like.Name,
                            Photo = like.Photo
                        });
                    }

                    foreach (var receiver in response.Receivers)
                    {
                        kr.Receivers.Add(new SuperKudos.Aggregator.Domain.Models.Person()
                        {
                            Id = new Guid(receiver.Id),
                            GivenName = receiver.GivenName,
                            Name = receiver.Name,
                            Photo = receiver.Photo
                        });
                    }

                    kudos.Add(kr);
                }
            }

           // kudos = await _restClientHelper.GetApiData<IEnumerable<KudosResponse>>($"{_gatewayServiceUrl}kudos/?pageNumber={pageNumber}");
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetKudos: {ex.Message}");
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

    public async Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber)
    {
        IEnumerable<KudosResponse> kudos = null;

        try
        {
            kudos = await _restClientHelper.GetApiData<IEnumerable<KudosResponse>>($"{_gatewayServiceUrl}kudosfromme/?userid={userId}&pageNumber={pageNumber}");
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return kudos;
    }

    public async Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber)
    {
        IEnumerable<KudosResponse> kudos = null;

        try
        {
            kudos = await _restClientHelper.GetApiData<IEnumerable<KudosResponse>>($"{_gatewayServiceUrl}kudosTome/?userid={userId}&pageNumber={pageNumber}");
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return kudos;
    }
}
