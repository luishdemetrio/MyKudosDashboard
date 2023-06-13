using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Concurrent;
using System.Text.Json;

namespace MyKudos.MSGraph.Api.Services;

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
    private GraphServiceClient _appClient;

    private ConcurrentDictionary<string, GraphUserPhoto> _userPhotos;

    private ConcurrentDictionary<string, string> _profilePictures;

    private ConcurrentDictionary<string, GraphUser> _userInfo;

    public GraphService(IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection("Settings").Get<Settings>();

        if (_clientSecretCredential == null)
        {
            _clientSecretCredential = new ClientSecretCredential(
                settings.TenantId, settings.ClientId, settings.ClientSecret);
        }
        
        _appClient = new GraphServiceClient(_clientSecretCredential,
            // Use the default scope, which will request the scopes
            // configured on the app registration
            new[] { "https://graph.microsoft.com/.default" });

        _userPhotos = new();

        _profilePictures = new();

        _userInfo = new();

    }

    public string GetAppOnlyTokenAsync()
    {
        // Ensure credential isn't null
        _ = _clientSecretCredential ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        // Request token with given scopes
        var context = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
        var response =  _clientSecretCredential.GetToken(context);
        return response.Token;
    }

    public  IEnumerable<GraphUser> GetUsers(string name, string emailDomain)
    {

        List<GraphUser> result = new();

        //var client = new RestClient($"https://graph.microsoft.com/v1.0/users/?$search=\"displayname:{name}\"&$select=id,displayname,userprincipalname");
        var client = new RestClient($"https://graph.microsoft.com/v1.0/users/?$count=true&$filter=endsWith(userPrincipalName,'{emailDomain}')&$search=\"displayname:{name}\"&$select=id,displayname,userprincipalname");


        var request = new RestRequest();

        request.Method = Method.Get;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer { GetAppOnlyTokenAsync()}");

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var users = JsonConvert.DeserializeObject<GraphUsers>(response.Content)!;

            if ((users != null) && (users.value != null))
                result.AddRange(users.value);

        }
        else
        {
            throw new Exception(response.Content);
        }

        return result;
    }

    public async Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] usersId)
    {

        List<GraphUserPhoto> photos = new();

        //we need to chunck the pending approvals to avoid getting an exception due the request is too long
        var chunckedUsersIds = usersId.Chunk(20);

        Parallel.ForEach(chunckedUsersIds, async p =>
        {
           
            photos.AddRange(await GetUserPhotosChunck(p));
        });

        return photos;

    }




    private async Task<IEnumerable<GraphUserPhoto>> GetUserPhotosChunck(string[] usersId)
    {
        List<GraphUserPhoto> photos = new();
        List<string> missingUsers = new();

        foreach (string userId in usersId)
        {
            if (_userPhotos.TryGetValue(userId, out GraphUserPhoto graphUserPhoto))
            {
                photos.Add(graphUserPhoto);
            }
            else
            {
                missingUsers.Add(userId);
            }
        }

        if (missingUsers.Count > 0)
        {
            var missingPhotos = await GetUserPhotosChunckGraph(missingUsers.ToArray());
            photos.AddRange(missingPhotos);

            foreach (var photo in missingPhotos)
            {
                _userPhotos.TryAdd(photo.id, photo);
            }
        }

        return photos;
    }

    private async Task<IEnumerable<GraphUserPhoto>> GetUserPhotosChunckGraph(string[] usersId)
    {

        List<GraphUserPhoto> photos = new();

        var client = new RestClient("https://graph.microsoft.com/v1.0/$batch");

        var request = new RestRequest();

        request.Method = Method.Post;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer {GetAppOnlyTokenAsync()}");


        List<GraphBatchRequestDTO> batch = new();

        foreach (var item in usersId)
        {
            batch.Add(new GraphBatchRequestDTO(item, "GET", $"users/{item}/photos/64x64/$value"));
        }

        var body = "{requests:" + JsonConvert.SerializeObject(batch) + "}";
        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var photosDTO = JsonConvert.DeserializeObject<GraphUserPhotos>(response.Content)!;

            foreach (var photo in photosDTO.responses.Where(p => p.status != "404"))
            {

                photos.Add(new GraphUserPhoto(photo.id, photo.body));
            }

        }

        return photos;
    }

    public async Task<string> GetUserManager(string userid)
    {

        string result = string.Empty;

        var directoryObject = await _appClient.Users[userid].Manager
                      .Request()
                      .Header("ConsistencyLevel", "eventual")
                      .Select("id")
                      .GetAsync();

        if (directoryObject != null)
        {
            result = directoryObject.Id;
        }

        return result;

    }


    public async Task<string> GetUserPhoto(string userid)
    {
        string profilePicture ;

        if (!_profilePictures.TryGetValue(userid, out profilePicture))
        {

            System.IO.Stream photo = await _appClient.Users[userid].Photos["48x48"].Content
            .Request()
            .GetAsync();

            using MemoryStream ms = new MemoryStream();
            photo.CopyTo(ms);

            profilePicture =  Convert.ToBase64String(ms.ToArray());
            
            _profilePictures.TryAdd(userid, profilePicture);
        }

        return profilePicture;
       
    }


    public async Task<List<GraphUser>> GetUserInfo(string[] users)
    {
   
        var result = new List<GraphUser>();

        List<string> missingUsers = new();

        foreach (string userId in users)
        {
            if (_userInfo.TryGetValue(userId, out GraphUser graphUserInfo))
            {
                result.Add(graphUserInfo);
            }
            else
            {
                missingUsers.Add(userId);
            }
        }

        if (missingUsers.Count > 0)
        {
            //we need to chunck the pending approvals to avoid getting an exception due the request is too long
            var chunckedUsersIds = users.Chunk(20);

            foreach (var userId in chunckedUsersIds)
            {

                var missingUserInfo = await GetUserInfoChunkGraph(missingUsers.ToArray());

                result.AddRange(missingUserInfo);

                foreach (var info in missingUserInfo)
                {
                    _userInfo.TryAdd(info.Id, info);
                }

            }

            
        }

        return result;
    }

    private async Task<List<GraphUser>> GetUserInfoChunkGraph(string[] users)
    {
        var result = new List<GraphUser>();

        var client = new RestClient("https://graph.microsoft.com/v1.0/$batch");

        var request = new RestRequest();

        request.Method = Method.Post;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer { GetAppOnlyTokenAsync()}");


        List<GraphBatchRequestDTO> batch = new();

        foreach (var item in users)
        {
            batch.Add(new GraphBatchRequestDTO(item, "GET", $"users/{item}/?$select=id,displayName"));
        }

        var body = "{requests:" + JsonConvert.SerializeObject(batch) + "}";
        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            using var items = JsonDocument.Parse(response.Content);


            foreach (var item in items.RootElement.EnumerateObject())
            {
                foreach (var user in item.Value.EnumerateArray())
                {
                    try
                    {
                        result.Add(new GraphUser()
                        {
                            Id = user.GetProperty("body").GetProperty("id").ToString(),
                            DisplayName = user.GetProperty("body").GetProperty("displayName").ToString()
                        });
                    }
                    catch { }
                    
                }

            }

        }

        return result;
    }


    


}