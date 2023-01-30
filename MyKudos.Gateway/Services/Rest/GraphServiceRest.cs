using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudos.Gateway.Services;

public class GraphServiceRest : IGraphService
{
       
    private readonly string _graphServiceUrl;
        
    public GraphServiceRest(IConfiguration configuration)
    {   
        _graphServiceUrl = configuration["graphServiceUrl"];

    }

    public Task<GraphUsers> GetUsers(string name)
    {
        GraphUsers r = new();

        var client = new RestClient($"{_graphServiceUrl}user/?name={name}");

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            r = JsonConvert.DeserializeObject<GraphUsers>(response.Content)!;

        }

        return Task.FromResult(r);
    }


    public Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] usersId)
    {

        List<GraphUserPhoto> photos = new(); 

        var client = new RestClient($"{_graphServiceUrl}photos");

        var request = new RestRequest();

        request.Method = Method.Get;

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(usersId);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            photos = JsonConvert.DeserializeObject<List<GraphUserPhoto>>(response.Content)!;

        }

        return Task.FromResult(photos.AsEnumerable());
    }

    public Task<string> GetUserPhoto(string userid)
    {
        string userPhoto = string.Empty;

        var client = new RestClient($"{_graphServiceUrl}photo/?userid={userid}");

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            userPhoto = JsonConvert.DeserializeObject<string>(response.Content);

        }

        return Task.FromResult(userPhoto);

    }

    public  Task<List<Models.GraphUser>> GetUserInfo(string[] users)
    {

        var result = new List<Models.GraphUser>();

        var client = new RestClient($"{_graphServiceUrl}userinfo");

        var request = new RestRequest();

        request.Method = Method.Get;

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(users);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<List<Models.GraphUser>>(response.Content)!;

        }

        return Task.FromResult(result);
    }

    public string GetUserManager(string userid)
    {
        string manager = string.Empty;

        var client = new RestClient($"{_graphServiceUrl}manager/?userid={userid}");

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            manager = JsonConvert.DeserializeObject<string>(response.Content);

        }

        return manager;
    }
}