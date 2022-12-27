using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.gRPC;
using static MyKudos.Kudos.gRPC.KudosService;

namespace MyKudos.Kudos.Api.Grpc;

public class KudosGrpc : KudosServiceBase
{

    private readonly IKudosService _kudosService;

    public KudosGrpc(IKudosService kudosService)
    {
        _kudosService = kudosService;
    }


    public override Task<PaginatedKudosResponse> GetKudos(Empty request, ServerCallContext context)
    {
        PaginatedKudosResponse kudos = new();

        var items = _kudosService.GetKudos();

        foreach (var item in items)
        {

            kudos.Data.Add(new KudosResponse()
            {
                 Id = item.Id.ToString(),
                FromPersonId = item.FromPersonId,
                ToPersonId = item.ToPersonId,
                 TitleId = item.TitleId,
                 Message = item.Message,
                 Date = item.Date.ToTimestamp()
            });

        }

        return Task.FromResult(kudos);
    }

    public override Task<SendKudosResponse> SendKudos(KudosRequest request, ServerCallContext context)
    {
       var result =  _kudosService.Send(new Domain.Models.KudosLog()
        {
            FromPersonId = request.FromPersonId,
            ToPersonId = request.ToPersonId,
            TitleId = request.TitleId,
            Message = request.Message,
            Date = request.SendOn.ToDateTime()
        });

        

        return Task.FromResult(new SendKudosResponse() { Succeed = result });

    }

    
}
