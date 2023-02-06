using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KudosController : ControllerBase
    {
        
        private readonly IKudosService _kudosService;

        public KudosController(IKudosService kudosService)
        {
            _kudosService = kudosService;
        }


        [HttpPost]
        public Guid Post([FromBody] KudosLog kudos)
        {

            Guid kudosId = _kudosService.Send(kudos);

            return kudosId;
        }

        [HttpGet(Name = "GetKudos")]
        public IEnumerable<KudosLog> Get()
        {
            return _kudosService.GetKudos();
        }

        //[HttpPost(Name = "CheckAndSeedDatabaseAsync")]
        //public async Task CheckAndSeedDatabaseAsync()
        //{
        //    var options = new DbContextOptionsBuilder<KudosDbContext>()
        //        .UseCosmos(
        //                "https://mykudos.documents.azure.com:443/",
        //                "pPT5EVtJyAh0Lk4N7ywHk2ZgPTSepeH6YvbUYw2R6msjLeCQLHMs1KfhOE5xPdoHUQVR3vMFiXvmACDbOWmCqA==",
        //                databaseName: "kudos-db")
        //        .Options;

        //    using var context = new KudosDbContext(options);

        //  //  var _ = await context.Database.EnsureDeletedAsync();

        //    if (await context.Database.EnsureCreatedAsync())
        //    {
        //        //context.Recognitions?.AddRange(Seed.Data);

        //        //await context.SaveChangesAsync();
        //    }

        //}
    }
}