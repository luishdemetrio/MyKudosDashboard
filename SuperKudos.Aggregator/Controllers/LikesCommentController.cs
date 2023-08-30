using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.Aggregator.Interfaces;

namespace SuperKudos.Aggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesCommentController : Controller
{

    private readonly ICommentsService _commentsService;

    public LikesCommentController(ICommentsService commentsService)
    {
        _commentsService = commentsService;
    }

    [HttpPost(Name = "SendLikeComment")]
    public Task<bool> SendLikeAsync([FromBody] LikeCommentGateway like)
    {
        

        return _commentsService.LikeCommentAsync(new KudosCatalog.Domain.Models.SendLike
        (
            KudosId: like.KudosId,
            FromPersonId: like.FromPerson.Id
        ));

    }

    [HttpDelete(Name = "UndolikeComment")]
    public Task<bool> Delete([FromBody] LikeCommentGateway unlike)
    {   

        return _commentsService.UndoLikeCommentAsync(new KudosCatalog.Domain.Models.SendLike
        (
            KudosId: unlike.KudosId,
            FromPersonId: unlike.FromPerson.Id
        ));



    }
}
