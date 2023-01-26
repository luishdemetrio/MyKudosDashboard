using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore.Storage;
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

            PaginatedPersonId likes = new();

            if (item.Likes != null)
            {
                foreach (var like in item.Likes)
                {
                    likes.Id.Add(new PersonId() { Id = like });
                }
            }
            

            kudos.Data.Add(new KudosResponse()
            {
                Id = item.Id.ToString(),
                FromPersonId = item.FromPersonId,
                ToPersonId = item.ToPersonId,
                TitleId = item.TitleId,
                Message = item.Message,
                Date = item.Date.ToTimestamp(),
                Likes = likes
            }) ; 

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


    public override Task<SendLikeResponse> SendLike(SendLikeRequest request, ServerCallContext context)
    {
        
        var r = _kudosService.SendLike(request.KudosId, request.PersonId);

        return Task.FromResult(new SendLikeResponse() { Succeed = r });

    }

}
