using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.Kudos.Token.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudos.Gateway.Services.Rest;

public class GamificationService : IGamificationService
{
 

    private readonly string _gamificationServiceUrl;
    private readonly IRestServiceToken _serviceToken;

    public GamificationService(IConfiguration config, IRestServiceToken serviceToken)
    {
        _gamificationServiceUrl = config["gamificationServiceUrl"];
        _serviceToken = serviceToken;
    }

    public async Task<IEnumerable<UserScore>> GetTopUserScoresAsync(int top)
    {
        List<UserScore> result = new();

        var client = new RestClient($"{_gamificationServiceUrl}Contributors?top={top}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<List<UserScore>>(response.Content);

        }

        return result;
    }

    public async Task<UserScore> GetUserScoreAsync(string pUserId)
    {
        UserScore result = new();

        var client = new RestClient($"{_gamificationServiceUrl}UserScore?userid={pUserId}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<UserScore>(response.Content);

        }

        return result;


    }
}
