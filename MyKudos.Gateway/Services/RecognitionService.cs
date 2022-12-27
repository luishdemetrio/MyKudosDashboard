using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using MyKudos.Gateway.Interfaces;
using static MyKudos.Recognition.gRPC.RecognitionService;

namespace MyKudos.Gateway.Services;

public class RecognitionService : IRecognitionService
{

    private readonly string _recognitionServiceUrl;

    public RecognitionService(IConfiguration config)
    {
        _recognitionServiceUrl = config["RecognitionServiceUrl"];
    }


    public IEnumerable<Models.Recognition> GetRecognitions()
    {
        var results = new List<Models.Recognition>();

        var channel = GrpcChannel.ForAddress(_recognitionServiceUrl);
        var client = new RecognitionServiceClient(channel);

        var items = client.GetRecognitions(new Empty());

        foreach (var item in items.Data)
        {
            results.Add(new Models.Recognition()
            {
                Id = item.Id,
                Emoji = item.Emoji,
                Description = item.Description
            });
        }

        return results;


    }
}
