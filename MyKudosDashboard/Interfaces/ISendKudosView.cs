using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ISendKudosView
{

    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();

    IEnumerable<UserViewModel> GetUsers(string name);

    string Send(KudosRequest kudos);

}