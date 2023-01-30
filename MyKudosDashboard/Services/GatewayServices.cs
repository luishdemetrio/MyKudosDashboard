using Microsoft.AspNetCore.Mvc;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;

namespace MyKudosDashboard.Services;

public class GatewayService : IGatewayService
{

    private readonly string _gatewayServiceUrl;

    public GatewayService(IConfiguration config)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
    }

    public IEnumerable<RecognitionViewModel> GetRecognitions()
    {

        List<RecognitionViewModel> recognitions = new();

        var uri = $"{_gatewayServiceUrl}recognition";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            recognitions = JsonConvert.DeserializeObject<IEnumerable<RecognitionViewModel>>(response.Content).ToList();

        }

        return recognitions;

    }


    public IEnumerable<KudosResponse> GetKudos()
    {
        List<KudosResponse> kudos = new();

        var uri = $"{_gatewayServiceUrl}kudos";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            kudos = JsonConvert.DeserializeObject<IEnumerable<KudosResponse>>(response.Content).ToList();
        }

        return kudos;
    }


    public bool SendKudos(KudosRequest kudos)
    {

        var uri = $"{_gatewayServiceUrl}kudos";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = Method.Post;

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(kudos);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);

    }

    public IEnumerable<UserViewModel> GetUsers(string name)
    {
        List<UserViewModel> kudos = new();

        var uri = $"{_gatewayServiceUrl}user/?name={name}";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            kudos = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(response.Content).ToList();
        }

        return kudos;
    }

    public Task<string> GetUserPhoto(string userid)
    {
        string userPhoto = string.Empty;

        var uri = $"{_gatewayServiceUrl}photo/?userid={userid}";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = Method.Get;

        RestResponse response = client.Execute(request);

        if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            userPhoto = "data:image/png;base64," + response.Content;//.Replace("\"", "").Replace("\\", "");
        }

        return Task.FromResult(userPhoto);
    }

    public bool SendLike(Like like)
    {
        var uri = $"{_gatewayServiceUrl}likes";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = Method.Post;

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");

        var body = JsonConvert.SerializeObject(like);

        request.AddParameter("application/json", body, ParameterType.RequestBody);


        RestResponse response = client.Execute(request);

        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
    }



}
