using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosToMyDirectsController : Controller
{
    private readonly IKudosService _kudosService;

    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;

    public KudosToMyDirectsController(IKudosService kudosService, IConfiguration configuration)
    {
        _kudosService = kudosService;

        _defaultPageNumber = int.Parse(configuration["DefaultPageNumber"]);
        _defaultPageSize = int.Parse(configuration["DefaultPageSize"]);
    }


    [HttpGet(Name = "GetKudosToMyDirects")]
    public Task<IEnumerable<Domain.Models.Kudos>> Get(Guid userId, int pageNumber, int pageSize)
    {
        if (pageNumber == 0)
            pageNumber = _defaultPageNumber;

        if (pageSize == 0)
            pageSize = _defaultPageSize;

        return _kudosService.GetKudosToMyDirectsAsync(userId, pageNumber, pageSize);
    }
}
