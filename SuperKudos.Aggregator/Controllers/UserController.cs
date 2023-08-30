using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.Aggregator.Domain.Models;

namespace SuperKudos.Aggregator.Controllers;

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
    public async Task<IEnumerable<Person>> GetUsersAsync(string name)
    {


        var result = new List<Person>();


        var users =  await _userProfileService.GetUsers(name);
       
        if (users.Count() == 0)
            return new List<Person>();

        foreach (var user in users)
        {
            result.Add(new Person
            {
                Id = user.UserProfileId,
                Name = user.DisplayName,
                Photo = string.IsNullOrEmpty(user?.Photo) ? _defaultProfilePicture : "data:image/png;base64," + user?.Photo
            });
        }


        return result;
        


    }

   
}
