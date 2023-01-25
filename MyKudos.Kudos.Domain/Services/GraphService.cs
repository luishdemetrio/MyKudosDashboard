//using Grpc.Net.Client;
//using Microsoft.Extensions.Configuration;
//using MyKudos.Kudos.Domain.Interfaces;
//using MyKudos.Kudos.Domain.Models;
//using MyKudos.MSGraph.gRPC;
//using static MyKudos.MSGraph.gRPC.MSGraphService;

//namespace MyKudos.Kudos.Domain.Services;

//public class GraphService : IGraphService
//{


//    private readonly string _graphServiceUrl;

//    public GraphService(IConfiguration configuration)
//    {
      
//        _graphServiceUrl = configuration["graphServiceUrl"];

//    }

//    public IEnumerable<GraphUserPhoto> GetUserPhotos(string[] usersId)
//    {

//        List<GraphUserPhoto> photos = new();

//        var msclient = new MSGraphServiceClient(
//                           GrpcChannel.ForAddress(_graphServiceUrl)
//                        );

//        var grpcPhotos = new ListUsersById();

//        foreach (var userId in usersId)
//        {
//            grpcPhotos.Ids.Add(new UserById() { Id = userId });
//        }

//        var grpcUsers = msclient.GetUserPhotos(grpcPhotos);


//        foreach (var user in grpcUsers.Users)
//        {
//            photos.Add(new GraphUserPhoto(user.Id, user.Photo));
//        }

//        return photos;
//    }


//    public string GetUserManager(string userid)
//    {
//        var msclient = new MSGraphServiceClient(
//                           GrpcChannel.ForAddress(_graphServiceUrl)
//                        );

//        var manager = msclient.GetUserManager(new UserById() { Id = userid });

//        return manager.Id;
//    }

//    public List<GraphUser> GetUserInfo(string[] usersId)
//    {

//        List<GraphUser> result = new();

//        var msclient = new MSGraphServiceClient(
//                          GrpcChannel.ForAddress(_graphServiceUrl)
//                       );

//        ListUsersById ids = new ListUsersById();

//        foreach (var item in usersId) {
//            ids.Ids.Add(new UserById() { Id = item });
//        }

//        var users = msclient.GetUserInfo(ids);

//        foreach (var user in users.User)
//        {
//            result.Add(new GraphUser()
//            {
//                Id = user.Id,
//                DisplayName = user.DisplayName,
//                UserPrincipalName = user.UserPrincipalName
//            });
//        }

//        return result;
//    }
//}
