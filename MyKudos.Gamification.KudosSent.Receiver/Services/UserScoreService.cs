using Microsoft.Extensions.Configuration;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Services;

public class UserScoreService : IUserScoreService
{


    private readonly string _userScoreServiceUrl;
     private readonly IRestServiceToken _serviceToken;

    public UserScoreService(IConfiguration config, IRestServiceToken serviceToken)
    {
        _serviceToken = serviceToken;
        _userScoreServiceUrl = config["userScoreServiceUrl"];
    }

    public async Task<UserScore> GetUserScoreAsync(string pUserId)
    {
        UserScore userScore = null;

        var uri = $"{_userScoreServiceUrl}UserScore/?userId={pUserId}";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");


        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            userScore = JsonConvert.DeserializeObject<UserScore>(response.Content);
        }

        return userScore;

    }

    public async Task<bool> SetUserScoreAsync(UserScore userScore)
    {
        var uri = $"{_userScoreServiceUrl}UserScore";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(userScore);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
    }
}
