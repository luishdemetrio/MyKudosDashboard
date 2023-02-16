using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudos.Gateway.Services;

public class GraphServiceRest : IGraphService
{
       
    private readonly string _graphServiceUrl;

    private readonly IRestServiceToken _serviceToken;

    public GraphServiceRest(IConfiguration configuration, IRestServiceToken serviceToken)
    {   
        _graphServiceUrl = configuration["graphServiceUrl"];

        _serviceToken = serviceToken;

    }

    public async Task<GraphUsers> GetUsers(string name)
    {
        GraphUsers r = new();

        var client = new RestClient($"{_graphServiceUrl}user/?name={name}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            r = JsonConvert.DeserializeObject<GraphUsers>(response.Content)!;

        }

        return r;
    }


    public async Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] usersId)
    {

        List<GraphUserPhoto> photos = new(); 

        var client = new RestClient($"{_graphServiceUrl}photos");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(usersId);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            photos = JsonConvert.DeserializeObject<List<GraphUserPhoto>>(response.Content)!;

        }

        return photos.AsEnumerable();
    }

    public async Task<string> GetUserPhoto(string userid)
    {
        string userPhoto = string.Empty;

        var client = new RestClient($"{_graphServiceUrl}photo/?userid={userid}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            userPhoto = JsonConvert.DeserializeObject<string>(response.Content);

        }

        return userPhoto;

    }

    public async Task<List<Models.GraphUser>> GetUserInfo(string[] users)
    {

        var result = new List<Models.GraphUser>();

        var client = new RestClient($"{_graphServiceUrl}userinfo");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(users);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<List<Models.GraphUser>>(response.Content)!;

        }

        return result;
    }

    public async Task<string> GetUserManagerAsync(string userid)
    {
        string manager = string.Empty;

        var client = new RestClient($"{_graphServiceUrl}manager/?userid={userid}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response.IsSuccessful)
        {
            manager = JsonConvert.DeserializeObject<string>(response.Content);

        }

        return manager;
    }
}