using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MyKudos.Recognition.gRPC;
using static MyKudos.Recognition.gRPC.RecognitionService;

namespace MyKudos.Gateway.Controllers;


[ApiController]
[Route("[controller]")]
public class RecognitionController : ControllerBase
{


    private readonly string _recognitionServiceUrl;

    public RecognitionController(IConfiguration config)
    {
        _recognitionServiceUrl = config["RecognitionServiceUrl"];
    }

    [HttpGet(Name = "GetRecognitions")]
    public IEnumerable<Models.Recognition> Get()
    {
        var results = new List<Models.Recognition>();

        var channel = GrpcChannel.ForAddress(_recognitionServiceUrl);
        var client = new RecognitionServiceClient(channel);

        var items = client.GetRecognitions(new RecognitionRequest());

        foreach (var item in items.Data)
        {
            results.Add(new Models.Recognition()
            {
                Emoji = item.Emoji,
                Description = item.Description
            });
        }

        return results;


    }



}