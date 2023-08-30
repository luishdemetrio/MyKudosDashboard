using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AllUsersController : Controller
{

    private readonly IGraphService _graphService;

    private readonly string[] _emailDomain;

    private readonly string _emailPrefixExclusion;

    private readonly IUserProfileRepository _userProfileRepository;

    

    public AllUsersController(IGraphService graphService, IConfiguration configuration,
                              IUserProfileRepository userProfileRepository)
    {
        _graphService = graphService;

        _emailDomain = configuration["EmailDomain"].ToString().Split(",");

        _userProfileRepository = userProfileRepository;


        _emailPrefixExclusion = configuration["EmailPrefixExclusion"];

    }

    [HttpPost(Name = "PopulateUserProfile")]
    public async Task<bool> PopulateUserProfile()
    {

        List<GraphUser> result = new();


        
        await _graphService.PopulateUserProfile(_userProfileRepository, _emailDomain, _emailPrefixExclusion);

        

        return true;
    }
}
