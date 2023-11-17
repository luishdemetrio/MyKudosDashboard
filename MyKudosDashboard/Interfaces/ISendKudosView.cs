using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface ISendKudosView
{

    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();

    Task<IEnumerable<Person>> GetUsersAsync(string name);

    Task<string> Send(SendKudosRequest kudos);

    

}