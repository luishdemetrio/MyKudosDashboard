using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosFromMeController : ControllerBase
{
    
    private readonly IKudosService _kudosService;

    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;

    public KudosFromMeController(IKudosService kudosService, IConfiguration configuration)
    {
        _kudosService = kudosService;

        _defaultPageNumber = int.Parse(configuration["DefaultPageNumber"]);
        _defaultPageSize = int.Parse(configuration["DefaultPageSize"]);
    }


    [HttpGet(Name = "GetKudosFromMeController")]
    public Task<IEnumerable<Domain.Models.Kudos>> Get(Guid userId, int pageNumber, int pageSize)
    {
        if (pageNumber == 0)
            pageNumber = _defaultPageNumber;

        if (pageSize == 0)
            pageSize = _defaultPageSize;

        return _kudosService.GetKudosFromMeAsync(userId, pageNumber, pageSize);
    }
}