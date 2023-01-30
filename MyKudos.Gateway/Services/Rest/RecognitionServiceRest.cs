using MyKudos.Gateway.Interfaces;
using Newtonsoft.Json;
using RestSharp;


namespace MyKudos.Gateway.Services;

public class RecognitionServiceRest : IRecognitionService
{

    private readonly string _recognitionServiceUrl;

    public RecognitionServiceRest(IConfiguration config)
    {
        _recognitionServiceUrl = config["RecognitionServiceUrl"];
    }


    public IEnumerable<Models.Recognition> GetRecognitions()
    {
        var results = new List<Models.Recognition>();

        var client = new RestClient($"{_recognitionServiceUrl}recognition");

        var request = new RestRequest();

        request.Method = Method.Get;
        
        RestResponse response = client.Execute(request);

        if (response != null && response.Content != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            results = JsonConvert.DeserializeObject<List<Models.Recognition>>(response.Content)!;

        }

        return results;


    }
}
