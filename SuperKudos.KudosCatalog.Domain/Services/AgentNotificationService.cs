//using Microsoft.Extensions.Configuration;
//using SuperKudos.KudosCatalog.Domain.Interfaces;
//using SuperKudos.KudosCatalog.Domain.Models;
//using Newtonsoft.Json;
//using RestSharp;

//namespace SuperKudos.KudosCatalog.Domain.Services;

//public class AgentNotificationService : IAgentNotificationService
//{

//    private readonly string _agentServiceUrl;
//    private readonly IGraphService _graphService;

//    public AgentNotificationService(IConfiguration config, IGraphService graphService)
//    {
//        _agentServiceUrl = config["agentServiceUrl"];
//        _graphService= graphService;
//    }

//    public bool SendNotification(KudosLog kudos)
//    {          

//        var uri = $"{_agentServiceUrl}api/notification";

//        var client = new RestClient(uri);

//        var request = new RestRequest();

//        request.Method = RestSharp.Method.Post;

//        request.AddHeader("Accept", "application/json");
//        request.AddHeader("Content-Type", "application/json");

//        KudosNotification notification =  GetNotificationInfoAsync(kudos);

//        var body = JsonConvert.SerializeObject(notification);

//        request.AddParameter("application/json", body, ParameterType.RequestBody);


//        RestResponse response = client.Execute(request);

//        return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);
        
//    }


//    private KudosNotification GetNotificationInfoAsync(KudosLog kudos)
//    {

//        KudosNotification result;

//        var photos = _graphService.GetUserPhotos(
//                            new string[]{ kudos.FromPersonId, kudos.ToPersonId});

//        string userManagerId = _graphService.GetUserManager(kudos.ToPersonId);

//        string[] usersId = { kudos.FromPersonId, kudos.ToPersonId, userManagerId };


//        var users = _graphService.GetUserInfo(usersId);

//        var userFrom = users.Where(u => u.Id == kudos.FromPersonId).FirstOrDefault();

//        var photosFrom = photos.Where(p => p.id == kudos.FromPersonId).FirstOrDefault();

//        var userTo = users.Where(u => u.Id == kudos.ToPersonId).FirstOrDefault();

//        var photosTo = photos.Where(p => p.id == kudos.ToPersonId).FirstOrDefault();

//        result = new KudosNotification(
//                         From: new Person() { Id = userFrom.Id, Name = userFrom.DisplayName, Photo = photosFrom.photo },
//                         To: new Person() { Id = userTo.Id, Name = userTo.DisplayName, Photo = photosTo.photo },
//                         Title: rec.Description,
//                         Message: kudos.Message,
//                         SendOn: kudos.Date,
//                         ManagerId: userManagerId
//                     );


//        //return result;


//        return null;
        
//    }



//}
