using Azure.Core;
using Azure.Identity;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Graph;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.MSGraph.gRPC;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;
using static MyKudos.Kudos.gRPC.KudosService;
using static MyKudos.MSGraph.gRPC.MSGraphService;
using static MyKudos.MSGraph.gRPC.MSGraphService;

namespace MyKudos.Gateway.Services;

public class Settings
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? TenantId { get; set; }
}

//public class GraphServiceGrpc : IGraphService
//{

//    // App-ony auth token credential
//    private ClientSecretCredential _clientSecretCredential;

//    // Client configured with app-only authentication
//    private GraphServiceClient _appClient;


//    private readonly string _graphServiceUrl;

    
//    public GraphServiceGrpc(IConfiguration configuration)
//    {
//        var settings = configuration.GetRequiredSection("Settings").Get<Settings>();

//        if (_clientSecretCredential == null)
//        {
//            _clientSecretCredential = new ClientSecretCredential(
//                settings.TenantId, settings.ClientId, settings.ClientSecret);
//        }
        
//        _appClient = new GraphServiceClient(_clientSecretCredential,
//            // Use the default scope, which will request the scopes
//            // configured on the app registration
//            new[] { "https://graph.microsoft.com/.default" });

//        _graphServiceUrl = configuration["graphServiceUrl"];

//    }

//    public async Task<string> GetAppOnlyTokenAsync()
//    {
//        // Ensure credential isn't null
//        _ = _clientSecretCredential ??
//            throw new NullReferenceException("Graph has not been initialized for app-only auth");

//        // Request token with given scopes
//        var context = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
//        var response = await _clientSecretCredential.GetTokenAsync(context);
//        return response.Token;
//    }


//    //get user's manager
//    //https://graph.microsoft.com/beta/me?$expand=manager($select=id)&$select=id,displayName,manager.id

//    public async Task<GraphUsers> GetUsers(string name)
//    {
//        GraphUsers r = new();

//        var msclient = new MSGraphServiceClient(
//                            GrpcChannel.ForAddress(_graphServiceUrl)
//                         );

//        var grpcUsers = msclient.GetUsers(new UserRequestByName() { Name = name });



//        foreach (var user in grpcUsers.Users)
//        {
//            r.value.Add(new Models.GraphUser()
//            {
//                Id = user.User.Id,
//                DisplayName = user.User.DisplayName,
//                UserPrincipalName = user.User.UserPrincipalName
//            });
//        }

//        return r;

//    }



//    public async Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] usersId)
//    {

//        List<GraphUserPhoto> photos = new();

//        var msclient = new MSGraphServiceClient(
//                           GrpcChannel.ForAddress(_graphServiceUrl)
//                        );

//        var grpcPhotos = new ListUsersById();

//        foreach (var userId in usersId)
//        {
//            grpcPhotos.Ids.Add(new UserById() { Id = userId } );
//        }
        
//        var grpcUsers = msclient.GetUserPhotos(grpcPhotos);


//        foreach (var user in grpcUsers.Users)
//        {
//            photos.Add(new GraphUserPhoto(user.Id, user.Photo));
//        }

//        return photos;
//    }

//    public async Task<string> GetUserPhoto(string userid)
//    {

//        System.IO.Stream photo = await _appClient.Users[userid].Photos["48x48"].Content
//            .Request()
//            .GetAsync();

//        using MemoryStream ms = new MemoryStream();
//        photo.CopyTo(ms);

//        return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

//    }

//    public async Task<List<Models.GraphUser>> GetUserInfo(string[] users)
//    {

//        var result = new List<Models.GraphUser>();

//        var client = new RestClient("https://graph.microsoft.com/v1.0/$batch");

//        var request = new RestRequest();

//        request.Method = Method.Post;
//        request.AddHeader("ConsistencyLevel", "eventual");
//        request.AddHeader("Authorization", $"Bearer {await GetAppOnlyTokenAsync()}");


//        List<GraphBatchRequest> batch = new();

//        foreach (var item in users)
//        {
//            batch.Add(new GraphBatchRequest(item, "GET", $"users/{item}/?$select=id,displayName"));
//        }

//        var body = "{requests:" + JsonConvert.SerializeObject(batch) + "}";
//        request.AddParameter("application/json", body, ParameterType.RequestBody);

//        RestResponse response = client.Execute(request);

//        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
//        {
//            using var items = JsonDocument.Parse(response.Content);


//            foreach (var item in items.RootElement.EnumerateObject())
//            {
//                foreach (var user in item.Value.EnumerateArray())
//                {
//                    result.Add(new GraphUser()
//                    {
//                        Id = user.GetProperty("body").GetProperty("id").ToString(),
//                        DisplayName = user.GetProperty("body").GetProperty("displayName").ToString()
//                    });
//                }

//            }

//        }

//        return result;
//    }

//    public string GetUserManagerAsync(string userid)
//    {
//        var msclient = new MSGraphServiceClient(
//                           GrpcChannel.ForAddress(_graphServiceUrl)
//                        );

//                var manager = msclient.GetUserManager(new UserById() { Id = userid });

//                return manager.Id;
//    }
//}