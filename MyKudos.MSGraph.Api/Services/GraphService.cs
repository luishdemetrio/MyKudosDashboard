using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using MyKudos.Kudos.Domain.Interfaces;

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


        _userInfo = new();

    }

    public string GetAppOnlyTokenAsync()
    {
        // Ensure credential isn't null
        _ = _clientSecretCredential ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        // Request token with given scopes
        var context = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
        var response = _clientSecretCredential.GetToken(context);
        return response.Token;
    }

    public IEnumerable<GraphUser> GetUsers(string name, string emailDomain)
    {

        List<GraphUser> result = new();

        //var client = new RestClient($"https://graph.microsoft.com/v1.0/users/?$search=\"displayname:{name}\"&$select=id,displayname,userprincipalname");
        var client = new RestClient($"https://graph.microsoft.com/v1.0/users/?$count=true&$filter=endsWith(userPrincipalName,'{emailDomain}')&$search=\"displayname:{name}\"&$select=id,displayname,userprincipalname");


        var request = new RestRequest();

        request.Method = Method.Get;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer {GetAppOnlyTokenAsync()}");

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

            //we need to chunck the pending approvals to avoid getting an exception due the request is too long
            var chunckedUsersIds = usersId.Chunk(20);

            foreach (var userId in chunckedUsersIds)
            {

                var missingPhotos = await GetUserPhotosChunckGraph(missingUsers.ToArray(), "48x48");
                photos.AddRange(missingPhotos);

                foreach (var photo in missingPhotos)
                {
                    _userPhotos.TryAdd(photo.id, photo);
                }

            }
        }

        return photos;
    }

    private async Task<IEnumerable<GraphUserPhoto>> GetUserPhotosChunckGraph(string[] usersId, string imageSize)
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
            batch.Add(new GraphBatchRequestDTO(item, "GET", $"users/{item}/photos/{imageSize}/$value"));
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

        try
        {
            var directoryObject = await _appClient.Users[userid].Manager
                      .Request()
                      .Header("ConsistencyLevel", "eventual")
                      .Select("id")
                      .GetAsync();

            if (directoryObject != null)
            {
                result = directoryObject.Id;
            }
        }
        catch { }


        return result;

    }


    public async Task<string> GetUserPhoto(string userid)
    {
        string profilePicture;

        System.IO.Stream photo = await _appClient.Users[userid].Photos["48x48"].Content
        .Request()
        .GetAsync();

        using MemoryStream ms = new MemoryStream();
        photo.CopyTo(ms);

        profilePicture = Convert.ToBase64String(ms.ToArray());



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
        request.AddHeader("Authorization", $"Bearer {GetAppOnlyTokenAsync()}");


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


    private async Task<List<GraphUserManager>> GetUserManagerChunkGraph(string[] users)
    {
        var result = new List<GraphUserManager>();

        var client = new RestClient("https://graph.microsoft.com/v1.0/$batch");

        var request = new RestRequest();

        request.Method = Method.Post;
        request.AddHeader("ConsistencyLevel", "eventual");
        request.AddHeader("Authorization", $"Bearer {GetAppOnlyTokenAsync()}");


        List<GraphBatchRequestDTO> batch = new();

        foreach (var item in users)
        {
            batch.Add(new GraphBatchRequestDTO(item, "GET", $"users/{item}/?$expand=manager($select=id,displayName)&$select=id,displayName"));
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
                        var graphUser = new GraphUserManager()
                        {
                            Id = user.GetProperty("body").GetProperty("id").ToString(),
                            DisplayName = user.GetProperty("body").GetProperty("displayName").ToString()
                        };

                        JsonElement manager;
                        if (user.GetProperty("body").TryGetProperty("manager", out manager))
                        {
                            // The manager property has a value

                            graphUser.ManagerId = new Guid(manager.GetProperty("id").ToString());   
                        }
                       

                        result.Add(graphUser);

                        
                    }
                    catch { }

                }

            }

        }

        return result;
    }


    public async Task<bool> PopulateUserProfile(IUserProfileRepository userProfileRepository, string[] domains)
    {

        var graphUsers = new Dictionary<Guid, MyKudos.Kudos.Domain.Models.UserProfile>();

        var usersPage = await _appClient.Users
          .Request()
          .Header("ConsistencyLevel", "eventual")
          .Select("id,displayName,UserPrincipalName,givenName, mail")
          .GetAsync();

        while (usersPage != null)
        {
            var userIds = new List<string>();

            foreach (var user in usersPage)
            {

                // Check if the user belongs to any of the specified domains
                if (domains.Any(domain => user.UserPrincipalName.EndsWith($"@{domain}")))
                {

                    var employee = new MyKudos.Kudos.Domain.Models.UserProfile
                    {
                        UserProfileId = new Guid(user.Id),
                        DisplayName = user.DisplayName.Length >= 60 ? user.DisplayName.Substring(0, 60) : user.DisplayName ,
                        GivenName = user.GivenName,
                        Mail = user.Mail
                        
                    };

                    if (!graphUsers.ContainsKey(employee.UserProfileId))
                    {
                        graphUsers.Add(employee.UserProfileId, employee);
                        userIds.Add(user.Id);
                    }

                }
            }

            // Get user photos in batches
            var batchSize = 20;

            for (int i = 0; i < userIds.Count; i += batchSize)
            {
                List<string> usersDictionary = new();

                for (int j = i; j < i + batchSize && j < userIds.Count; j++)
                {
                    usersDictionary.Add(userIds[j]);
                }

                var photos96x96 = await GetUserPhotosChunckGraph(usersDictionary.ToArray(), "96x96");


                foreach (var photo in photos96x96)
                {


                    if (graphUsers.TryGetValue(new Guid(photo.id), out var user))
                    {

                        user.Photo96x96 = photo.photo;

                    }
                }

                var photos48x48 = await GetUserPhotosChunckGraph(usersDictionary.ToArray(), "48x48");


                foreach (var photo in photos48x48)
                {


                    if (graphUsers.TryGetValue(new Guid(photo.id), out var user))
                    {

                        user.Photo = photo.photo;

                    }
                }


                var managers = await GetUserManagerChunkGraph(usersDictionary.ToArray());

                foreach (var manager in managers)
                {


                    if ((manager.ManagerId != null) && graphUsers.TryGetValue(new Guid(manager.Id), out var user))
                    {

                        user.ManagerId = manager.ManagerId;

                    }
                }
            }

            if (usersPage.NextPageRequest != null)
            {
                usersPage = await usersPage.NextPageRequest.GetAsync();
            }
            else
            {
                usersPage = null;
            }
        }

        userProfileRepository.PopulateUserProfile(graphUsers.Values.ToList());


        return true;


    }

}