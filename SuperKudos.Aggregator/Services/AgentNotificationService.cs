using MyKudos.Communication.Helper.Interfaces;
using SuperKudos.Aggregator.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace SuperKudos.Aggregator.Services;

public class AgentNotificationService : IAgentNotificationService
{

    private readonly string _agentServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<KudosServiceRest> _logger;

    public AgentNotificationService(IConfiguration config, IRestClientHelper clientHelper, ILogger<KudosServiceRest> logger)
    {
        _agentServiceUrl = config["agentServiceUrl"];
        _restClientHelper = clientHelper;
        _logger = logger;
    }

    public async Task<bool> SendNotificationAsync(Aggregator.Domain.Models.KudosNotification kudos)
    {

        bool result = false;

        try
        {

            var uri = $"{_agentServiceUrl}";

            var client = new RestClient(uri);

            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var body = JsonConvert.SerializeObject(kudos);

            request.AddParameter("application/json", body, ParameterType.RequestBody);


            RestResponse response = client.Execute(request);

            return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);

            // result = await _restClientHelper.SendApiData<Kudos.Domain.Models.KudosNotification, bool>($"{_agentServiceUrl}api/notification", HttpMethod.Post, kudos);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return result;
    }



}
