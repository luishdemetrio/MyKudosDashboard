using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.Kudos.gRPC;
using static MyKudos.Kudos.gRPC.KudosService;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : Controller
{
    private readonly string _kudosServiceUrl;

    private readonly IGraphService _graphService;

    public KudosController(IConfiguration config, IGraphService graphService)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _graphService = graphService;
    }

    [HttpGet(Name = "GetKudos")]
    public async Task<IEnumerable<Models.Kudos>> Get()
    {
       // var kudos = new List<Models.Kudos>();

        var client = new KudosServiceClient(
                            GrpcChannel.ForAddress(_kudosServiceUrl)
                         );

        var kudos = client.GetKudos(new Empty());

        var from= kudos.Data.Select(u => u.FromPersonId).Distinct().ToList();

        from.AddRange(kudos.Data.Select(u =>u.ToPersonId).Distinct());


        List<GraphUser> users =  await _graphService.GetUserInfoAsync(from.Distinct().ToArray()).ConfigureAwait(true);
        

        var result = from kudo in kudos.Data
                     join userTo in users
                        on kudo.ToPersonId equals userTo.Id
                     join userFrom in users
                        on kudo.FromPersonId equals userFrom.Id
                    select new Models.Kudos(
                        id : kudo.Id,
                        SentBy: userFrom.DisplayName,
                        SentTo: userTo.DisplayName,
                        Title: kudo.TitleId,
                        Message: kudo.Message,
                        SendOn: DateTime.Now
                    );



                return result;


    }

    private void GetEmployeeNames()
    {

    }
}
