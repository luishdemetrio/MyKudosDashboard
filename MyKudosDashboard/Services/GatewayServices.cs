using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.Identity.Client;

namespace MyKudosDashboard.Services;

public class GatewayService : IGatewayService
{

    private readonly string _gatewayServiceUrl;

    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _tenantId;
    private readonly string _exposedAPI;
    private readonly IConfidentialClientApplication _confidentialClientApplication;

    public GatewayService(IConfiguration config)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];

        _clientId = config["ClientId"];
        _clientSecret = config["ClientSecret"];
        _tenantId = config["TenantId"];
        _exposedAPI = config["ExposedApi"];

        _confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(_clientId)
        .WithClientSecret(_clientSecret)
        .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}"))
        .Build();

    }


    private async Task<string> GetAccessTokenAsync()
    {
        
        var scopes = new string[] { _exposedAPI };

        var result =  await _confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();

        return result.AccessToken;
    }

    public async Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync()
    {

        List<RecognitionViewModel> recognitions = new();

        var uri = $"{_gatewayServiceUrl}recognition";

        var client = new RestClient(uri);
                
        var token = await GetAccessTokenAsync();

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


    public string SendKudos(KudosRequest kudos)
    {
        string kudosId = string.Empty;

        var uri = $"{_gatewayServiceUrl}kudos";

        var client = new RestClient(uri);

        var request = new RestRequest();

        request.Method = Method.Post;

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
            userPhoto = JsonConvert.DeserializeObject<string>(response.Content);
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
