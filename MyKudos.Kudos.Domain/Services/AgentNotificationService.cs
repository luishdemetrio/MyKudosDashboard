using Microsoft.Extensions.Configuration;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudos.Kudos.Domain.Services;

public class AgentNotificationService : IAgentNotificationService
{

    private readonly string _agentServiceUrl;

    public AgentNotificationService(IConfiguration config)
    {
        _agentServiceUrl = config["agentServiceUrl"];
    }

    public bool SendNotification(KudosLog kudos)
    {          

        var uri = $"{_agentServiceUrl}api/notification";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = RestSharp.Method.Post;

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(kudos);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
        
    }

   
}
