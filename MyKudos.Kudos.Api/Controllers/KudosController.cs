using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : ControllerBase
{
    
    private readonly IKudosService _kudosService;

    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;

    public KudosController(IKudosService kudosService, IConfiguration configuration)
    {
        _kudosService = kudosService;

        _defaultPageNumber = int.Parse(configuration["DefaultPageNumber"]);
        _defaultPageSize = int.Parse(configuration["DefaultPageSize"]);
    }


    [HttpPost]
    public int Post([FromBody] Domain.Models.Kudos kudos)
    {
        return _kudosService.Send(kudos);
    }

    [HttpGet(Name = "GetKudos")]
    public Task<IEnumerable<Domain.Models.Kudos>> Get(int pageNumber, int pageSize)
    {
        if (pageNumber == 0)
            pageNumber = _defaultPageNumber;

        if (pageSize == 0)
            pageSize = _defaultPageSize;

        return _kudosService.GetKudos(pageNumber, pageSize);
    }

    [HttpPut(Name = "UpdateKudos")]
    public  bool UpdateKudos([FromBody] Domain.Models.KudosMessage     kudos)
    {
        return  _kudosService.UpdateKudos(kudos.KudosId, kudos.Message);
    }

    [HttpDelete(Name = "DeleteKudos")]
    public  bool DeleteKudos([FromBody] int kudosId)
    {
        return  _kudosService.DeleteKudos(kudosId);
    }

}