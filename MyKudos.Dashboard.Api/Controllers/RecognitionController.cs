using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyKudos.Dashboard.App.Interfaces;
using MyKudos.Dashboard.Data.Context;
using MyKudos.Dashboard.Data.Data;
using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecognitionController : ControllerBase
{

    private readonly IRecognitionService _recognitionService;


    public RecognitionController(IRecognitionService recognitionService)
    {
        _recognitionService = recognitionService;
    }

    [HttpGet(Name = "GetRecognitions")]
    public IEnumerable<Domain.Models.Recognition> Get()
    {
        return _recognitionService.GetRecognitions();
    }

    //[HttpPost(Name = "CheckAndSeedDatabaseAsync")]
    //public async Task CheckAndSeedDatabaseAsync()
    //{
    //        var options = new DbContextOptionsBuilder<RecognitionDbContext>()
    //            .UseCosmos(
    //                    "https://mykudos.documents.azure.com:443/",
    //                    "pPT5EVtJyAh0Lk4N7ywHk2ZgPTSepeH6YvbUYw2R6msjLeCQLHMs1KfhOE5xPdoHUQVR3vMFiXvmACDbOWmCqA==",
    //                    databaseName: "dashboard-db")
    //            .Options;

    //        using var context = new RecognitionDbContext(options);

    //        var _ = await context.Database.EnsureDeletedAsync();

    //        if (await context.Database.EnsureCreatedAsync())
    //        {
    //            context.Recognitions?.AddRange(Seed.Data);

    //            await context.SaveChangesAsync();
    //        }
 
    //}
}