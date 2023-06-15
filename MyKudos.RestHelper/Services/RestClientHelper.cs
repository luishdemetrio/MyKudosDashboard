using MyKudos.Communication.Helper.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MyKudos.Communication.Helper.Services;

public class RestClientHelper : IRestClientHelper
{

    private readonly IRestServiceToken _serviceToken;

    public RestClientHelper(IRestServiceToken restServiceToken)
    {
        _serviceToken = restServiceToken;
    }


    private async Task SetCommonHeaders(HttpRequestMessage request)
    {
        var token = await _serviceToken.GetAccessTokenAsync();

        request.Headers.Add("Authorization", "Bearer " + token);
        request.Headers.Add("Accept", "application/json");
     
    }

    public async Task<TResponse> GetApiData<TResponse>(string endpoint)
    {

      
            using var httpClient = new HttpClient();

            using var request = new HttpRequestMessage(new HttpMethod("GET"), endpoint);

            await SetCommonHeaders(request);

   
            using var response = await httpClient.SendAsync(request);

            
            var responseContent = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        return JsonConvert.DeserializeObject<TResponse>(responseContent, settings);

      
    }

    public async Task<TResponse> GetApiData<TRequestBody, TResponse>( string endpoint, TRequestBody? body)
    {

       return await SendApiData<TRequestBody, TResponse>(endpoint, HttpMethod.Get, body);
      
    }



    public async Task<TResponse> SendApiData<TRequestBody, TResponse>(string endpoint, HttpMethod httpMethod, TRequestBody body)
    {

        using var httpClient = new HttpClient();

        using var request = new HttpRequestMessage(new HttpMethod(httpMethod.Method), endpoint);

        await SetCommonHeaders(request);

        if (body != null)
        {
            var requestBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");


            request.Content = content;
        }

        using var response = await httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        return JsonConvert.DeserializeObject<TResponse>(responseContent);


        
    }


}