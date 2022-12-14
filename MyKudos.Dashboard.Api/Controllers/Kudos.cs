using Microsoft.AspNetCore.Mvc;
using MyKudos.Dashboard.App.Interface;
using MyKudos.Dashboard.Domain.Models;


namespace MyKudos.Dashboard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class Kudos : ControllerBase
{

    private readonly IRecognitionService _recognitionService;


    public Kudos(IRecognitionService recognitionService)
    {
        _recognitionService = recognitionService;
    }

    [HttpGet(Name = "GetAccounts")]
    public IEnumerable<Recognition> Get()
    {
        return _recognitionService.GetRecognitions();
    }

}