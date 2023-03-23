using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Kudos.Token.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudos.Gateway.Services;

public class KudosServiceRest: IKudosService
{

    private readonly string _kudosServiceUrl;
    private readonly IRestServiceToken _serviceToken;

    public KudosServiceRest(IConfiguration config, IRestServiceToken serviceToken)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _serviceToken = serviceToken;
    }

    public async Task<IEnumerable<Comments>> GetComments(string kudosId)
    {
        List<Comments> result = new();

        var client = new RestClient($"{_kudosServiceUrl}Comments?kudosId={kudosId}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<List<Comments>>(response.Content)!;

        }

        return result;
    }

    public async Task<IEnumerable<Models.Kudos>> GetKudosAsync()
    {

        List<Models.Kudos> result = new();

        var client = new RestClient($"{_kudosServiceUrl}kudos");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<List<Models.Kudos>>(response.Content)!;

        }

        return result;


    }

    public async Task<string> SendAsync(Models.KudosRequest kudos)
    {
        var result = string.Empty;

        var client = new RestClient($"{_kudosServiceUrl}kudos");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");


        var restKudos = new KudosLog()
        {
            FromPersonId = kudos.From.Id,
            ToPersonId = kudos.To.Id,
            TitleId = kudos.Title.Id,   
            Message = kudos.Message,
            Date = kudos.SendOn
        };

        var body = JsonConvert.SerializeObject(restKudos);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<string>(response.Content);
        }

        return result;
    }

    public async Task<string> SendCommentsAsync(Comments comment)
    {
        string result =string.Empty;

        var client = new RestClient($"{_kudosServiceUrl}Comments");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(comment);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);



        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<string>(response.Content);
        }

        return result;
    }

    public async Task<int> SendLikeAsync(LikeGateway like)
    {
        int result = 0;

        var client = new RestClient($"{_kudosServiceUrl}like");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(like);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<int>(response.Content);
        }

        return result;
    }
}
