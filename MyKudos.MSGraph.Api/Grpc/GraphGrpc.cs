using Grpc.Core;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.gRPC;
using static MyKudos.MSGraph.gRPC.MSGraphService;

namespace MyKudos.MSGraph.Api.Grpc;

public class GraphGrpc : MSGraphServiceBase
{
    private readonly IGraphService _graphService;

	public GraphGrpc(IGraphService graphService)
	{
		_graphService= graphService;
	}

    public override async Task<ListUsersByName> GetUsers(UserRequestByName request, 
                                                           ServerCallContext context)
    {
        var users =  await _graphService.GetUsers(request.Name);

        var responseUsers = new ListUsersByName();

        foreach (var user in users.value)
        {
            responseUsers.Users.Add(new UserResponseByName()
            {
                 User = new MSGraphUser()
                 {
                      Id= user.Id,
                      DisplayName= user.DisplayName,
                      UserPrincipalName= user.UserPrincipalName
                 }
            });
        }
        return responseUsers;

    }

    public override async Task<UserPhotosList> GetUserPhotos(ListUsersById request, ServerCallContext context)
    {

        UserPhotosList result = new();

        string[] users = request.Ids.Select(u => u.Id).ToArray();


        var photos = await _graphService.GetUserPhotos(users);

        foreach (var photo in photos)
        {
            result.Users.Add(new UserPhoto()
            {
                    Id = photo.id,
                    Photo = photo.photo
            });
        }

        return result;


    }
}
