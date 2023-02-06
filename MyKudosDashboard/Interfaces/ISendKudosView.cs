using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ISendKudosView
{

    IEnumerable<RecognitionViewModel> GetRecognitions();

    IEnumerable<UserViewModel> GetUsers(string name);

    Guid Send(KudosRequest kudos);

}