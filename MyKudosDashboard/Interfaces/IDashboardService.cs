using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IDashboardService
{
    IEnumerable<RecognitionViewModel> GetRecognitions();

    bool SendKudos(KudosViewModel kudos);
}
