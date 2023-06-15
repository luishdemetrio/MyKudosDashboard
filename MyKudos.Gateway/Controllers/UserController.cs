using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;


namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{


    //private IGraphService _graphService;

    private IUserProfileService _userProfileService;

    private string _defaultProfilePicture;

    public UserController(IConfiguration configuration, IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];
    }


    [HttpGet(Name = "GetUser/{name}")]
    public async Task<IEnumerable<Gateway.Domain.Models.Person>> GetUsersAsync(string name)
    {


        var result = new List<Gateway.Domain.Models.Person>();


        var users =  await _userProfileService.GetUsers(name);
       
        if (users.Count() == 0)
            return new List<Gateway.Domain.Models.Person>();

        foreach (var user in users)
        {
            result.Add(new Gateway.Domain.Models.Person
            {
                Id = user.UserProfileId,
                Name = user.DisplayName,
                Photo = string.IsNullOrEmpty(user?.Photo) ? _defaultProfilePicture : "data:image/png;base64," + user?.Photo
            });
        }


        return result;
        


    }

   
}
