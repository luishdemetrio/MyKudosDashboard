using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosToMeController : ControllerBase
{
    
    private readonly IKudosService _kudosService;

    private readonly int _defaultPageNumber;
    private readonly int _defaultPageSize;

    public KudosToMeController(IKudosService kudosService, IConfiguration configuration)
    {
        _kudosService = kudosService;

        _defaultPageNumber = int.Parse(configuration["DefaultPageNumber"]);
        _defaultPageSize = int.Parse(configuration["DefaultPageSize"]);
    }


    [HttpGet(Name = "GetKudosToMeController")]
    public Task<IEnumerable<Domain.Models.Kudos>> Get(string userId, int pageNumber, int pageSize)
    {
        if (pageNumber == 0)
            pageNumber = _defaultPageNumber;

        if (pageSize == 0)
            pageSize = _defaultPageSize;

        return _kudosService.GetKudosToMeAsync(userId, pageNumber, pageSize);
    }
}