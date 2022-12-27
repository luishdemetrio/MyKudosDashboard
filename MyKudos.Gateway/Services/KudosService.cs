using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.Graph;
using Microsoft.Graph.Extensions;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using System;
using static MyKudos.Kudos.gRPC.KudosService;

namespace MyKudos.Gateway.Services;

public class KudosService : IKudosService
{

    private readonly string _kudosServiceUrl;

    public KudosService(IConfiguration config)
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
            result.Add(new Models.Kudos(
                Id: item.Id,
                From:  item.FromPersonId,
                To:  item.ToPersonId,
                TitleId: item.TitleId,
                Message: item.Message,
                SendOn: item.Date.ToDateTime()
                ));
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
            FromPersonId = kudos.From,
            ToPersonId = kudos.To,
            Message = kudos.Message,
            TitleId = kudos.TitleId,
            SendOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToTimestamp()
        }) ;

        return r.Succeed;
    }


}
