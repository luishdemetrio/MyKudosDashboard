

namespace MyKudos.Gateway.Services;

//public class RecognitionServiceGrpc : IRecognitionService
//{

//    private readonly string _recognitionServiceUrl;

//    public RecognitionServiceGrpc(IConfiguration config)
//    {
//        _recognitionServiceUrl = config["RecognitionServiceUrl"];
//    }


//    public IEnumerable<Models.Recognition> GetRecognitionsAsync()
//    {
//        var results = new List<Models.Recognition>();

//        var channel = GrpcChannel.ForAddress(_recognitionServiceUrl);
//        var client = new RecognitionServiceClient(channel);

//        var items = client.GetRecognitions(new Empty());

//        foreach (var item in items.Data)
//        {
//            results.Add(new Models.Recognition()
//            {
//                Id = item.Id,
//                Emoji = item.Emoji,
//                Description = item.Description
//            });
//        }

//        return results;


//    }
//}
