using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGatewayService
{
    IEnumerable<RecognitionViewModel> GetRecognitions();

    bool SendKudos(KudosViewModel kudos);
}
