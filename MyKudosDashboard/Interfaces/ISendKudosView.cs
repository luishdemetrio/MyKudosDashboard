using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ISendKudosView
{

    IEnumerable<RecognitionViewModel> GetRecognitions();

    Task<IEnumerable<UserViewModel>> GetUsersAsync(string name);

    bool Send(KudosViewModel kudos);

}