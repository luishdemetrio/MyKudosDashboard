﻿using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudosDashboard.Services;

public class GatewayService : IGatewayService
{

    private readonly string _gatewayServiceUrl;
    private readonly IRestServiceToken _serviceToken;

    public GatewayService(IConfiguration config, IRestServiceToken restServiceToken)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _serviceToken = restServiceToken;
        
    }


    public async Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync()
    {

        List<RecognitionViewModel> recognitions = new();

        var uri = $"{_gatewayServiceUrl}recognition";

        var client = new RestClient(uri);
                
        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);


        RestResponse response = client.Execute(request);

        if (response.IsSuccessful)
        {
            recognitions = JsonConvert.DeserializeObject<IEnumerable<RecognitionViewModel>>(response.Content).ToList();

        }

        return recognitions;

    }


    public async Task<IEnumerable<KudosResponse>> GetKudos()
    {
        List<KudosResponse> kudos = new();

        var uri = $"{_gatewayServiceUrl}kudos";

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
            kudos = JsonConvert.DeserializeObject<IEnumerable<KudosResponse>>(response.Content).ToList();
        }

        return kudos;
    }


    public async Task<string> SendKudos(KudosRequest kudos)
    {
        string kudosId = string.Empty;

        var uri = $"{_gatewayServiceUrl}kudos";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(kudos);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            kudosId = JsonConvert.DeserializeObject<string>(response.Content);
        }
        
        return kudosId;

    }

    public async Task<IEnumerable<UserViewModel>> GetUsers(string name)
    {
        List<UserViewModel> kudos = new();

        var uri = $"{_gatewayServiceUrl}user/?name={name}";

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
            kudos = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(response.Content).ToList();
        }

        return kudos;
    }

    public async Task<string> GetUserPhoto(string userid)
    {
        string userPhoto = string.Empty;

        var uri = $"{_gatewayServiceUrl}photo/?userid={userid}";

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
            userPhoto = JsonConvert.DeserializeObject<string>(response.Content);
        }

        return userPhoto;
    }

    public async Task<bool> Like(Like like)
    {
        var uri = $"{_gatewayServiceUrl}likes";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(like);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
    }

    public async Task<bool> UndoLike(Like like)
    {
        var uri = $"{_gatewayServiceUrl}likes";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Delete;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(like);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
    }


    public async Task<bool> LikeComment(LikeComment like)
    {
        var uri = $"{_gatewayServiceUrl}likescomment";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(like);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
    }

    public async Task<bool> UndoLikeComment(LikeComment like)
    {
        var uri = $"{_gatewayServiceUrl}likescomment";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Delete;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(like);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
    }

    public async Task<UserScore> GetUserScoreAsync(string pUserId)
    {
        UserScore result = new();

        var client = new RestClient($"{_gatewayServiceUrl}gamification?userid={pUserId}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<UserScore>(response.Content)!;

        }

        return result;
    }


    public async Task<IEnumerable<TopContributors>> GetTopContributors()
    {
        List<TopContributors> result = new();

        var client = new RestClient($"{_gatewayServiceUrl}contributors");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<IEnumerable<TopContributors>>(response.Content).ToList();

        }

        return result;
    }

    public async Task<string> SendCommentsAsync(CommentsRequest comment)
    {
        string result = string.Empty;

        var uri = $"{_gatewayServiceUrl}Comments";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(comment);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        comment.Id = "new";
        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<string>(response.Content);

        }

        return result;
    }

    public async Task<IEnumerable<CommentsResponse>> GetComments(string kudosId)
    {
        List<CommentsResponse> result = new();

        var client = new RestClient($"{_gatewayServiceUrl}Comments?kudosId={kudosId}");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<IEnumerable<CommentsResponse>>(response.Content).ToList();

        }

        return result;
    }

    public async Task<bool> UpdateComments(CommentsRequest comments)
    {
        bool result = false;

        var client = new RestClient($"{_gatewayServiceUrl}Comments");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Put;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(comments);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<bool>(response.Content);
        }

        return result;
    }

    public async Task<bool> DeleteComments(CommentsRequest comments)
    {
        bool result = false;

        var client = new RestClient($"{_gatewayServiceUrl}Comments");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Delete;
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(comments);

        request.AddParameter("application/json", body, ParameterType.RequestBody);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<bool>(response.Content);
        }

        return result;
    }
}
