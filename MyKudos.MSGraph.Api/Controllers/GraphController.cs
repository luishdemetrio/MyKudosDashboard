using Microsoft.AspNetCore.Mvc;

namespace MyKudos.MSGraph.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphController : ControllerBase
    {
        
        private readonly ILogger<GraphController> _logger;

        public GraphController(ILogger<GraphController> logger)
        {
            _logger = logger;
        }

    }
}