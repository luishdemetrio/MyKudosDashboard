using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface ISendKudosView
{

    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();

    Task<IEnumerable<Person>> GetUserProfile(string name);

    Task<UserProfile> GetUserById(string userId);

    Task<string> Send(SendKudosRequest kudos);

    

}