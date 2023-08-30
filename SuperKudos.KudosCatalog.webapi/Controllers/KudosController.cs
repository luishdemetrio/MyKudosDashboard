using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

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
}