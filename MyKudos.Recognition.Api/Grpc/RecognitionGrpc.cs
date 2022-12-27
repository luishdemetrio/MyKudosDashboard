using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MyKudos.Recognition.App.Interfaces;
using MyKudos.Recognition.gRPC;
using static MyKudos.Recognition.gRPC.RecognitionService;

namespace MyKudos.Recognition.Api.Grpc;

public class RecognitionGrpc : RecognitionServiceBase
{

    private readonly IRecognitionService _recognitionService;


    public RecognitionGrpc(IRecognitionService recognitionService)
    {
        _recognitionService = recognitionService;
    }

    
    public override Task<PaginatedRecognitionsResponse> GetRecognitions(Empty request, ServerCallContext context)
    {
        PaginatedRecognitionsResponse recognitions = new();

        var items = _recognitionService.GetRecognitions();

        foreach (var item in items)
        {

            recognitions.Data.Add(new RecognitionResponse()
            {
                Id = item.Id.ToString(),
                Emoji = item.Emoji,
                Description = item.Description
            });

        }

        return Task.FromResult(recognitions);
    }


}
