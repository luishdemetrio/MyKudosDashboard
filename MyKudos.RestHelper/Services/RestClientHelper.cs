using MyKudos.Communication.Helper.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace MyKudos.Communication.Helper.Services;

public class RestClientHelper : IRestClientHelper
{

    private readonly IRestServiceToken _serviceToken;

    public RestClientHelper(IRestServiceToken restServiceToken)
    {
        _serviceToken = restServiceToken;
    }

    private async Task SetCommonHeadersAsync(RestRequest request)
    {
        var token = await _serviceToken.GetAccessTokenAsync();

        request.AddHeader("Authorization", "Bearer " + token);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");
    }

    public async Task<TResponse> GetApiData<TResponse>(string endpoint)
    {
        var client = new RestClient(endpoint);

        var request = new RestRequest();
        request.Method = Method.Get;

        await SetCommonHeadersAsync(request);

        var response = await client.ExecuteAsync(request);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return JsonConvert.DeserializeObject<TResponse>(response.Content);
            // handle other status codes here
            default:
                throw new Exception($"API Error: {response.StatusCode}");
        }
    }

    public async Task<TResponse> GetApiData<TRequestBody, TResponse>( string endpoint, TRequestBody? body)
    {
        var client = new RestClient(endpoint);

        var request = new RestRequest();
        request.Method = Method.Get;

        await SetCommonHeadersAsync(request);

        if (body != null)
        {
            var requestBody = JsonConvert.SerializeObject(body);
            request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
        }

        var response = await client.ExecuteAsync(request);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return JsonConvert.DeserializeObject<TResponse>(response.Content);
            // handle other status codes here
            default:
                throw new Exception($"API Error: {response.StatusCode}");
        }
    }



    public async Task<TResponse> SendApiData<TRequestBody, TResponse>(string endpoint, Method httpMethod, TRequestBody body)
    {
        var client = new RestClient(endpoint);

        var request = new RestRequest();
        request.Method = httpMethod;

        await SetCommonHeadersAsync(request);

        var requestBody = JsonConvert.SerializeObject(body);

        request.AddParameter("application/json", requestBody, ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return JsonConvert.DeserializeObject<TResponse>(response.Content);
            // handle other status codes here
            default:
                throw new Exception($"API Error: {response.StatusCode}");
        }
    }


}