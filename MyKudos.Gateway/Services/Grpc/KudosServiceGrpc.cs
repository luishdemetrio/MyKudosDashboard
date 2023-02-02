using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.Kudos.gRPC;
using static MyKudos.Kudos.gRPC.KudosService;

namespace MyKudos.Gateway.Services;

public class KudosServiceGrpc : IKudosService
{

    private readonly string _kudosServiceUrl;

    public KudosServiceGrpc(IConfiguration config)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
    }

    public IEnumerable<Models.Kudos> GetKudos()
    {

        List<Models.Kudos> result = new();

        var client = new KudosServiceClient(
                            GrpcChannel.ForAddress(_kudosServiceUrl)
                         );

        var kudos = client.GetKudos(new Empty());

        foreach (var item in kudos.Data)
        {
            result.Add(new Models.Kudos() {
                Id = item.Id,
                FromPersonId = item.FromPersonId,
                ToPersonId = item.ToPersonId,
                TitleId = item.TitleId,
                Message = item.Message,
                Date = item.Date.ToDateTime(),
                Likes = item.Likes.Id.Select(x => x.Id).ToList()
                }) ;
        }


        return result;


    }

    public bool Send(Models.KudosRequest kudos)
    {
        
        var client = new KudosServiceClient(
                            GrpcChannel.ForAddress(_kudosServiceUrl)
                         );

        var r = client.SendKudos(new Kudos.gRPC.KudosRequest()
        {
            FromPersonId = kudos.From.Id,
            ToPersonId = kudos.To.Id,
            Message = kudos.Message,
            TitleId = kudos.Title.Id,
            SendOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToTimestamp()
        }) ;

        return r.Succeed;
    }

    public bool SendLike(LikeGateway like)
    {
        List<Models.Kudos> result = new();

        var client = new KudosServiceClient(
                            GrpcChannel.ForAddress(_kudosServiceUrl)
                         );

        var r = client.SendLike(new Kudos.gRPC.SendLikeRequest()
        {
            KudosId = like.KudosId,
            PersonId = like.PersonId
        });

        return r.Succeed;

    }
}
