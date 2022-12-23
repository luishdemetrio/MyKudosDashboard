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

        var client = new KudosServiceClient(
                            GrpcChannel.ForAddress(_kudosServiceUrl)
                         );

        var items = client.GetKudos(new KudosRequest());

        var from= items.Data.Select(u => u.FromPersonId).Distinct().ToList();

        from.AddRange(items.Data.Select(u =>u.ToPersonId).Distinct());

        from.Distinct().ToArray();


        foreach (var item in items.Data)
        {
            
        }

        return results;


    }

    private void GetEmployeeNames()
    {

    }
}
