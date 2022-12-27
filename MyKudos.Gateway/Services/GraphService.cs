using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Graph;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;

namespace MyKudos.Gateway.Services;

public class Settings
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? TenantId { get; set; }
}

public class GraphService : IGraphService
{

    // App-ony auth token credential
    private ClientSecretCredential _clientSecretCredential;

    // Client configured with app-only authentication
    private static GraphServiceClient _appClient;

    public GraphService(IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection("Settings").Get<Settings>();

        if (_clientSecretCredential == null)
        {
            _clientSecretCredential = new ClientSecretCredential(
                settings.TenantId, settings.ClientId, settings.ClientSecret);
        }

        if (_appClient == null)
        {
            _appClient = new GraphServiceClient(_clientSecretCredential,
                // Use the default scope, which will request the scopes
                // configured on the app registration
                new[] { "https://graph.microsoft.com/.default" });
        }
    }

    public async Task<string> GetAppOnlyTokenAsync()
    {
        // Ensure credential isn't null
        _ = _clientSecretCredential ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        // Request token with given scopes
        var context = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
        var response = await _clientSecretCredential.GetTokenAsync(context);
        return response.Token;
    }



    public async Task<GraphUsers> GetUsers(string name)
    {

        GraphUsers r = new();

        var client = new RestClient($"https://graph.microsoft.com/v1.0/users/?$search=\"displayname:{name}\"&$select=id,displayname,userprincipalname");

        var request = new RestRequest();

        request.Method = Method.Get;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer {await GetAppOnlyTokenAsync()}");

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            r = JsonConvert.DeserializeObject<GraphUsers>(response.Content)!;

        }

        return r;
    }



    public async Task<GraphUserPhotos> GetUserPhotos(GraphUsers users)
    {

        GraphUserPhotos photos = new();

        var client = new RestClient("https://graph.microsoft.com/v1.0/$batch");

        var request = new RestRequest();

        request.Method = Method.Post;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer {await GetAppOnlyTokenAsync()}");


        List<GraphBatchRequest> batch = new();

        foreach (var item in users.value)
        {
            batch.Add(new GraphBatchRequest(item.Id, "GET", $"users/{item.Id}/photos/48x48/$value"));
        }

        var body = "{requests:" + JsonConvert.SerializeObject(batch) + "}";
        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            photos = JsonConvert.DeserializeObject<GraphUserPhotos>(response.Content)!;


        }

        return photos;
    }

    public async Task<string> GetUserPhoto(string userid)
    {


        System.IO.Stream photo = await _appClient.Users[userid].Photos["48x48"].Content
            .Request()
            .GetAsync();

        using MemoryStream ms = new MemoryStream();
        photo.CopyTo(ms);

        return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

    }

    public async Task<List<GraphUser>> GetUserInfoAsync(string[] users)
    {

        var result = new List<GraphUser>();

        var client = new RestClient("https://graph.microsoft.com/v1.0/$batch");

        var request = new RestRequest();

        request.Method = Method.Post;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer {await GetAppOnlyTokenAsync()}");


        List<GraphBatchRequest> batch = new();

        foreach (var item in users)
        {
            batch.Add(new GraphBatchRequest(item, "GET", $"users/{item}/?$select=id,displayName"));
        }

        var body = "{requests:" + JsonConvert.SerializeObject(batch) + "}";
        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            using var items = JsonDocument.Parse(response.Content);


            foreach (var item in items.RootElement.EnumerateObject())
            {
                foreach (var user in item.Value.EnumerateArray())
                {
                    result.Add(new GraphUser()
                    {
                        Id = user.GetProperty("body").GetProperty("id").ToString(),
                        DisplayName = user.GetProperty("body").GetProperty("displayName").ToString()
                    });
                }

            }

        }

        return result;
    }


    //public class Rootobject
    //{
    //    public Respons[] responses { get; set; }
    //}

    //public class Respons
    //{
    //    public Body body { get; set; }
    //}

    

    //public class Body
    //{
    //    public string id { get; set; }
    //    public string displayName { get; set; }
    //}


}