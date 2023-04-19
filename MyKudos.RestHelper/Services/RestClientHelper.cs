using MyKudos.Communication.Helper.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MyKudos.Communication.Helper.Services;

public class RestClientHelper : IRestClientHelper
{

    private readonly IRestServiceToken _serviceToken;

    public RestClientHelper(IRestServiceToken restServiceToken)
    {
        _serviceToken = restServiceToken;
    }

    //private async Task SetCommonHeadersAsync(RestRequest request)
    //{
    //    var token = await _serviceToken.GetAccessTokenAsync();

    //    request.AddHeader("Authorization", "Bearer " + token);
    //    request.AddHeader("Accept", "application/json");
    //    request.AddHeader("Content-Type", "application/json");
    //}

    private async Task SetCommonHeaders(HttpRequestMessage request)
    {
        var token = await _serviceToken.GetAccessTokenAsync();

        request.Headers.Add("Authorization", "Bearer " + token);
        request.Headers.Add("Accept", "application/json");
        //request.Headers.Add("Content-Type", "application/json");
    }

    public async Task<TResponse> GetApiData<TResponse>(string endpoint)
    {

      
            using var httpClient = new HttpClient();

            using var request = new HttpRequestMessage(new HttpMethod("GET"), endpoint);

            await SetCommonHeaders(request);

            using var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
    
            return JsonConvert.DeserializeObject<TResponse>(responseContent);

        //    return myResponse;

       



        //if (ex.StatusCode == HttpStatusCode.NotFound)
        //{
        //    Console.WriteLine("Resource not found");
        //}
        //else
        //{
        //    Console.WriteLine($"Request failed: {ex.StatusCode}");
        //}

        //if (ex.InnerException != null)
        //{
        //    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        //}

        //if (ex.Response != null)
        //{
        //    var errorContent = await ex.Response.Content.ReadAsStringAsync();
        //    Console.WriteLine($"Error response content: {errorContent}");
        //}

        //throw;
    }

    public async Task<TResponse> GetApiData<TRequestBody, TResponse>( string endpoint, TRequestBody? body)
    {


       return await SendApiData<TRequestBody, TResponse>(endpoint, HttpMethod.Get, body);

        //using var httpClient = new HttpClient();

        //using var request = new HttpRequestMessage(new HttpMethod("GET"), endpoint);

        //await SetCommonHeaders(request);

        //if (body != null)
        //{
        //    var requestBody = JsonConvert.SerializeObject(body);
        //    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");


        //    request.Content = content;
        //}

        //using var response = await httpClient.SendAsync(request);

        //response.EnsureSuccessStatusCode();

        //var responseContent = await response.Content.ReadAsStringAsync();

        //return JsonConvert.DeserializeObject<TResponse>(responseContent );

        //var client = new RestClient(endpoint);

        //var request = new RestRequest();
        //request.Method = Method.Get;

        //await SetCommonHeadersAsync(request);

        //if (body != null)
        //{
        //    var requestBody = JsonConvert.SerializeObject(body);
        //    request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
        //}

        //var response = await client.ExecuteAsync(request);

        //switch (response.StatusCode)
        //{
        //    case HttpStatusCode.OK:
        //        return JsonConvert.DeserializeObject<TResponse>(response.Content);
        //    // handle other status codes here
        //    default:
        //        throw new Exception($"API Error: {response.StatusCode}");
        //}
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

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(responseContent);


        //var client = new RestClient(endpoint);

        //var request = new RestRequest();
        //request.Method = httpMethod;

        //await SetCommonHeadersAsync(request);

        //var requestBody = JsonConvert.SerializeObject(body);

        //request.AddParameter("application/json", requestBody, ParameterType.RequestBody);

        //var response = await client.ExecuteAsync(request);

        //switch (response.StatusCode)
        //{
        //    case HttpStatusCode.OK:
        //        return JsonConvert.DeserializeObject<TResponse>(response.Content);
        //    // handle other status codes here
        //    default:
        //        throw new Exception($"API Error: {response.StatusCode}");
        //}
    }


}