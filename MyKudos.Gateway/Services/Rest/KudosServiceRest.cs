using Microsoft.Graph;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudos.Gateway.Services;

public class KudosServiceRest: IKudosService
{

    private readonly string _kudosServiceUrl;

    public KudosServiceRest(IConfiguration config)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
    }

    public IEnumerable<Models.Kudos> GetKudos()
    {

        List<Models.Kudos> result = new();

        var client = new RestClient($"{_kudosServiceUrl}kudos");

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<List<Models.Kudos>>(response.Content)!;

        }

        return result;


    }

    public bool Send(Models.KudosRequest kudos)
    {

        var client = new RestClient($"{_kudosServiceUrl}kudos");

        var request = new RestRequest();

        request.Method = Method.Post;

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(kudos);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
    }

    public bool SendLike(Like like)
    {
        var client = new RestClient($"{_kudosServiceUrl}like");

        var request = new RestRequest();

        request.Method = Method.Post;

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(like);
        
        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);

    }
}
