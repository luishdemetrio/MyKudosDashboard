using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ISendKudosView
{

    Task<IEnumerable<Recognition>> GetRecognitionsAsync();

    Task<IEnumerable<UserViewModel>> GetUsersAsync(string name);

    Task<string> Send(KudosRequest kudos);

}