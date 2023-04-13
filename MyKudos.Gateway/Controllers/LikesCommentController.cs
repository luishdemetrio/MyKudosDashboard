using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.Gateway.Services;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesCommentController : Controller
{

    private readonly IKudosService _kudosService;

    public LikesCommentController(IKudosService kudosService)
    {
        _kudosService = kudosService;
    }

    [HttpPost(Name = "SendLikeComment")]
    public Task<bool> SendLikeAsync([FromBody] LikeCommentGateway like)
    {
        

        return _kudosService.LikeCommentAsync(new Kudos.Domain.Models.SendLike
        (
            KudosId: like.KudosId,
            FromPersonId: like.FromPerson.Id
        ));

    }

    [HttpDelete(Name = "UndolikeComment")]
    public Task<bool> Delete([FromBody] LikeCommentGateway unlike)
    {   

        return _kudosService.UndoLikeCommentAsync(new Kudos.Domain.Models.SendLike
        (
            KudosId: unlike.KudosId,
            FromPersonId: unlike.FromPerson.Id
        ));



    }
}
