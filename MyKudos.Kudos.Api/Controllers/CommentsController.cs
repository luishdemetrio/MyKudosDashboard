using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class CommentsController : Controller
{
    private readonly IKudosService _kudosService;

    public CommentsController(IKudosService kudosService)
    {
        _kudosService = kudosService;
    }

    [HttpPost(Name = "SendMessage")]
    public string Post(Comments comments)
    {

        return _kudosService.SendComments(comments);
    }


    [HttpGet(Name = "GetComments")]
    public IEnumerable<Comments> Get(string kudosId)
    {
        return _kudosService.GetComments(kudosId);
    }



    [HttpPut(Name = "Update")]
    public bool Put(Comments comments)
    {
        return _kudosService.UpdateComments(comments);
    }

    [HttpDelete(Name = "Delete")]
    public bool Delete(string kudosId, string commentId)
    {
        return _kudosService.DeleteComments(kudosId, Guid.Parse(commentId));
    }
}

