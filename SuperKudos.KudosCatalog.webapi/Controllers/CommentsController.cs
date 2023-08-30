using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;


[ApiController]
[Route("[controller]")]
public class CommentsController : Controller
{
    private readonly ICommentsService _commentsService;

    public CommentsController(ICommentsService commentsService)
    {
        _commentsService = commentsService;
    }

    [HttpPost(Name = "SendMessage")]
    public int Post(Comments comments)
    {

        return _commentsService.SendComments(comments);
    }


    [HttpGet(Name = "GetComments")]
    public IEnumerable<Comments> Get(int kudosId)
    {
        return _commentsService.GetComments(kudosId);
    }



    [HttpPut(Name = "Update")]
    public bool Put(Comments comments)
    {
        return _commentsService.UpdateComments(comments);
    }

    [HttpDelete(Name = "Delete")]
    public bool Delete(int kudosId, int commentId)
    {
        return _commentsService.DeleteComments(kudosId, commentId);
    }
}

