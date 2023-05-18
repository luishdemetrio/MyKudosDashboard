using Microsoft.Extensions.Configuration;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Kudos.Notification.Receiver.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace MyKudos.Kudos.Notification.Receiver;

public class AgentNotificationService : IAgentNotificationService
{

    private readonly string _agentServiceUrl;
    private readonly IRestServiceToken _serviceToken;

    public AgentNotificationService(IConfiguration config, IRestServiceToken serviceToken)
    {
        _agentServiceUrl = config["agentServiceUrl"];
        _serviceToken = serviceToken;
    }

    public async Task<bool> SendNotificationAsync(KudosNotification kudos)
    {          

        var uri = $"{_agentServiceUrl}";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(kudos);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
        
    }

   

}
