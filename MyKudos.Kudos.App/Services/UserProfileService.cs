using Microsoft.Extensions.Configuration;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Services;

public class UserProfileService : IUserProfileService
{

    private IUserProfileRepository _userProfileRepository;

    private IRestClientHelper _restClientHelper;

    private string _graphServiceUrl;

    public UserProfileService(IUserProfileRepository userProfileRepository,
                              IRestClientHelper restClientHelper,
                              IConfiguration configuration)
    {
        _userProfileRepository = userProfileRepository;
        _restClientHelper = restClientHelper;

        _graphServiceUrl = configuration["graphServiceUrl"];
    }

    public bool AddUser(UserProfile user)
    {
        return _userProfileRepository.Add(user);
    }

    public List<UserProfile> GetAllUsers()
    {
        return _userProfileRepository.GetAll();
    }

    public bool PopulateUserProfile()
    {
        var graphUsers = GetUsersViaGraph();

        return true;
    }


    private async Task<List<UserProfile>> GetUsersViaGraph() 
    {

        List<UserProfile> result = new();

        
            var kudos = await _restClientHelper.GetApiData<IEnumerable<UserProfile>>($"{_graphServiceUrl}allusers");
            result = kudos.ToList();
        
        return result;
    }

    public List<UserProfile> GetUsers(string name)
    {
        return _userProfileRepository.GetUsers(name);
    }
}
