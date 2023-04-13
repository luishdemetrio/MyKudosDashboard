using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesController : Controller
{

    private readonly IKudosService _kudosService;

    private IKudosQueue _kudosQueue;

    public LikesController(IKudosService kudosService, IKudosQueue kudosQueue)
    {

        _kudosService = kudosService;

        _kudosQueue = kudosQueue;
    }

    [HttpPost(Name = "SendLike")]
    public  Task<bool> SendLikeAsync([FromBody] LikeGateway like)
    {
        _ = _kudosQueue.SendLikeAsync(like);

        return _kudosService.LikeAsync(new Kudos.Domain.Models.SendLike
        (
            KudosId : like.KudosId,
            FromPersonId : like.FromPerson.Id
        ));

    }

    [HttpDelete(Name = "Undolike")]
    public Task<bool> Delete([FromBody] LikeGateway unlike)
    {
        _ = _kudosQueue.SendDislikeAsync(unlike);

        return _kudosService.UndoLikeAsync(new Kudos.Domain.Models.SendLike
        (
            KudosId: unlike.KudosId,
            FromPersonId: unlike.FromPerson.Id
        ));



    }
}
