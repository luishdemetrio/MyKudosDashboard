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
    public async Task<int> PostAsync([FromBody] SendKudosRequest kudos)
    {


        var restKudos = new Kudos.Domain.Models.Kudos()
        {
            FromPersonId = kudos.FromPersonId,
            ToPersonId = kudos.ToPersonId,
            RecognitionId = kudos.RecognitionId,
            Message = kudos.Message,
            Date = kudos.Date
            
        };

        //Save the Kudos in the database
        int kudosId = await _kudosService.SendAsync(restKudos);

        //get the manager of the person that received kudos. That will be used later to send the adaptive card to the manager
        Guid userManagerId = await _graphService.GetUserManagerAsync(kudos.ToPersonId);

        //get missing information like photos to send the adaptive card and notifications

        var kudosDb = await _kudosService.GetKudosUser(kudosId);
        
        if (kudosDb != null)
        {
            //send the kudos notification to the Teams Dashboard
            var queue = _kudosQueue.SendKudosAsync(kudosId, new GatewayDomain.KudosNotification(
              From: new Person { Id = kudosDb.UserFrom.UserProfileId, Name = kudosDb.UserFrom.DisplayName, Photo = $"data:image/png;base64,{kudosDb.UserFrom.Photo}" },
              To: new Person { Id = kudosDb.UserTo.UserProfileId, Name = kudosDb.UserTo.DisplayName, Photo = $"data:image/png;base64,{kudosDb.UserTo.Photo96x96}" },
              ManagerId: userManagerId,
              Message: kudosDb.Message,
              Reward:  new Reward ( Id : kudosDb.Recognition.RecognitionId,Title : kudosDb.Recognition.Title ),
              SendOn: kudosDb.Date
              ));

            //send the adaptive card
            await _agentNotificationService.SendNotificationAsync(
                new Kudos.Domain.Models.KudosNotification(
                      From: new Kudos.Domain.Models.Person()
                      {
                          Id = kudosDb.UserFrom.UserProfileId,
                          Name = kudosDb.UserFrom.DisplayName,
                          Photo = $"data:image/png;base64,{kudosDb.UserFrom.Photo}"
                      }
                      ,
                      To: new Kudos.Domain.Models.Person()
                      {
                          Id = kudosDb.UserTo.UserProfileId,
                          Name = kudosDb.UserTo.DisplayName,
                          Photo = $"data:image/png;base64,{kudosDb.UserTo.Photo}"
                      },
                      ManagerId: userManagerId,
                      Message: kudosDb.Message,
                      Reward: new Kudos.Domain.Models.Reward(kudosDb.Recognition.RecognitionId, kudosDb.Recognition.Title),
                      SendOn: kudosDb.Date)
                );

            Task.WaitAll(queue);
        }

       


        //we need to use 48x48 to reduce size of the Teams message (28K at total)
        //var fromPhoto48x48 = await _graphService.GetUserPhoto(kudos.From.Id);
      //  var toPhoto48x48 = await _graphService.GetUserPhoto(kudos.To.Id);

       

        return kudosId;
    }

    


}
