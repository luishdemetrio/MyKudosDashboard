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
        kudos.SentOnYear = DateTime.Today.Year;

        return _kudosService.Send(kudos);
    }

    [HttpGet(Name = "GetKudos")]
    public Task<IEnumerable<Domain.Models.Kudos>> Get(int pageNumber, int pageSize, 
                                                      Guid? managerId = null, int? sentOnYear = null)
    {
        if (pageNumber == 0)
            pageNumber = _defaultPageNumber;

        if (pageSize == 0)
            pageSize = _defaultPageSize;

        return _kudosService.GetKudos(pageNumber, pageSize, managerId, sentOnYear);
    }

    [HttpGet("GetKudosByName/{name},{pageSize},{fromNumberOfDays}")]
    public Task<IEnumerable<Domain.Models.Kudos>> Get(string name, int pageSize, int fromNumberOfDays)
    {

        if (pageSize == 0)
            pageSize = _defaultPageSize;

        return _kudosService.GetKudosByName(name, pageSize, fromNumberOfDays);
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