using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers
{
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
        public Guid Post([FromBody] KudosLog kudos)
        {
            Guid kudosId = _kudosService.Send(kudos);

            return kudosId;
        }

        [HttpGet(Name = "GetKudos")]
        public Task<IEnumerable<KudosLog>> Get(int pageNumber, int pageSize)
        {
            if (pageNumber == 0)
                pageNumber = _defaultPageNumber;

            if (pageSize == 0)
                pageSize = _defaultPageSize;

            return _kudosService.GetKudos(pageNumber, pageSize);
        }
    }
}