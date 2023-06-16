using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : Controller
{
    
    private readonly IGraphService _graphService;
    private readonly IKudosService _kudosService;

    private readonly IAgentNotificationService _agentNotificationService;

    private readonly IUserProfileService _userProfileService;

    private string _defaultProfilePicture;

    private IKudosMessageSender _kudosQueue;

    
    public KudosController(IGraphService graphService, 
                           IKudosService kudosService, IKudosMessageSender kudosQueue,
                           IConfiguration configuration,
                           IAgentNotificationService agentNotificationService)
    {
        
        _graphService = graphService;
        _kudosService = kudosService;
        _agentNotificationService = agentNotificationService;

        _kudosQueue = kudosQueue;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];

    }


    [HttpGet(Name = "GetKudos")]
    public async Task<IEnumerable<KudosResponse>> Get(int pageNumber = 1)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosAsync(pageNumber);


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
                Likes = kudo.Likes.Where(l=> l.Person != null).Select(x => new GatewayDomain.Person()
                {
                    Id = x.Person.UserProfileId,
                    Name = x.Person.DisplayName,
                    Photo = x.Person.Photo != null ? $"data:image/png;base64,{x.Person.Photo}" : _defaultProfilePicture
                }).ToList(),
                Comments = (kudo.Comments is null) ? new List<int>() : kudo.Comments.Select(c => c.CommentsId).ToList() ,
                Message = kudo.Message,
                SendOn = kudo.Date,
                Title = kudo.Recognition.Title

            });
        }

        return result;

    }


    [HttpPost(Name = "SendKudos")]
    public async Task<int> PostAsync([FromBody] KudosRequest kudos)
    {


        var restKudos = new Kudos.Domain.Models.Kudos()
        {
            FromPersonId = kudos.From.Id,
            ToPersonId = kudos.To.Id,
            RecognitionId = kudos.Reward.Id,
            Message = kudos.Message,
            Date = kudos.SendOn,
            
        };

        //Save the Kudos in the database
        int kudosId = await _kudosService.SendAsync(restKudos);

        //get the manager of the person that received kudos. That will be used later to send the adaptive card to the manager
        Guid userManagerId = await _graphService.GetUserManagerAsync(kudos.To.Id);

        //send the kudos notification to the Teams Dashboard
        var queue = _kudosQueue.SendKudosAsync(kudosId, new GatewayDomain.KudosNotification(
          From: kudos.From,
          To: kudos.To,
          ManagerId: userManagerId,
          Message: kudos.Message,
          Reward: kudos.Reward,
          SendOn: kudos.SendOn
          ));


        //we need to use 48x48 to reduce size of the Teams message (28K at total)
        var fromPhoto48x48 = await _graphService.GetUserPhoto(kudos.From.Id);
        var toPhoto48x48 = await _graphService.GetUserPhoto(kudos.To.Id);

        //send the adaptive card
        await _agentNotificationService.SendNotificationAsync(
            new Kudos.Domain.Models.KudosNotification(
                  From: new Kudos.Domain.Models.Person() { Id = kudos.From.Id, Name = kudos.From.Name, Photo = $"data:image/png;base64,{fromPhoto48x48}" },
                  To: new Kudos.Domain.Models.Person() { Id = kudos.To.Id, Name = kudos.To.Name, Photo = $"data:image/png;base64,{toPhoto48x48}" },
                  ManagerId: userManagerId,
                  Message: kudos.Message,
                  Reward: new Kudos.Domain.Models.Reward( kudos.Reward.Id, kudos.Reward.Title),
                  SendOn: kudos.SendOn)
            );

        Task.WaitAll(queue);

        return kudosId;
    }

    


}
