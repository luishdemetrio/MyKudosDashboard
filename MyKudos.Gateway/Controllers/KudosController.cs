using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.gRPC;
using static MyKudos.Kudos.gRPC.KudosService;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : Controller
{
    private readonly string _kudosServiceUrl;

    public KudosController(IConfiguration config)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
    }

    [HttpGet(Name = "GetKudos")]
    public IEnumerable<Models.Kudos> Get()
    {
        var results = new List<Models.Kudos>();

        var channel = GrpcChannel.ForAddress(_kudosServiceUrl);
        var client = new KudosServiceClient(channel);

        var items = client.GetKudos(new KudosRequest());

        foreach (var item in items.Data)
        {
            
        }

        return results;


    }
}
