using SuperKudos.Aggregator.Domain.Models;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ISendKudosView
{

    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();

    Task<IEnumerable<UserViewModel>> GetUsersAsync(string name);

    Task<string> Send(SendKudosRequest kudos);

    

}