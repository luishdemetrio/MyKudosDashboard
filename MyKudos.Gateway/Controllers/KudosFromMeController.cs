using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;
using Microsoft.Graph;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosFromMeController : Controller
{
    
    private readonly IGraphService _graphService;
    private readonly IRecognitionService _recognitionService;
    private readonly IKudosService _kudosService;

    private string _defaultProfilePicture;

    private IEnumerable<GatewayDomain.Recognition> _recognitions;

    
    public KudosFromMeController(IGraphService graphService, IRecognitionService recognitionService, 
                                 IKudosService kudosService, IConfiguration configuration)
    {
        
        _graphService = graphService;
        _recognitionService = recognitionService;
        _kudosService = kudosService;

        _ = PopulateRecognitionsAsync();

        _defaultProfilePicture = configuration["DefaultProfilePicture"];

    }

    private async Task PopulateRecognitionsAsync()
    {
        _recognitions = await _recognitionService.GetRecognitionsAsync().ConfigureAwait(false);
    }


    [HttpGet(Name = "GetKudosFromMe")]
    public async Task<IEnumerable<KudosResponse>> Get(string userId, int pageNumber = 1)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosFromMeAsync(userId, pageNumber);

        var result = new List<KudosResponse>();

        foreach (var kudo in kudos)
        {

            result.Add(new KudosResponse()
            {
                Id = kudo.KudosId,
                From = new GatewayDomain.Person()
                {
                    Id = kudo.UserFrom.UserProfileId,
                    Name = kudo.UserFrom.DisplayName,
                    Photo = kudo.UserFrom.Photo != null ? $"data:image/png;base64,{kudo.UserFrom.Photo}" : _defaultProfilePicture
                },

                To = new GatewayDomain.Person()
                {
                    Id = kudo.UserTo.UserProfileId,
                    Name = kudo.UserTo.DisplayName,
                    Photo = kudo.UserTo.Photo96x96 != null ? $"data:image/png;base64,{kudo.UserTo.Photo96x96}" : _defaultProfilePicture
                },
                Likes = kudo.Likes.Where(l => l.Person != null).Select(x => new GatewayDomain.Person()
                {
                    Id = x.Person.UserProfileId,
                    Name = x.Person.DisplayName,
                    Photo = x.Person.Photo != null ? $"data:image/png;base64,{x.Person.Photo}" : _defaultProfilePicture
                }).ToList(),
                Comments = (kudo.Comments is null) ? new List<int>() : kudo.Comments.Select(c => c.CommentsId).ToList(),
                Message = kudo.Message,
                SendOn = kudo.Date,
                Title = kudo.Recognition.Title

            });
        }

        return result;

    }


}
