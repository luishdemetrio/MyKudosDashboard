using MyKudos.Gateway.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace MyKudos.Gateway.Services;

public class RecognitionServiceRest : IRecognitionService
{

    private readonly string _recognitionServiceUrl;
    private readonly IRestServiceToken _serviceToken;

    public RecognitionServiceRest(IConfiguration config, IRestServiceToken serviceToken)
    {
        _recognitionServiceUrl = config["RecognitionServiceUrl"];

        _serviceToken= serviceToken;

    }

    public async Task<IEnumerable<Models.Recognition>> GetRecognitionsAsync()
    {
        var results = new List<Models.Recognition>();

        var client = new RestClient($"{_recognitionServiceUrl}recognition");

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);

        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            results = JsonConvert.DeserializeObject<List<Models.Recognition>>(response.Content)!;

        }

        return results;


    }
}
