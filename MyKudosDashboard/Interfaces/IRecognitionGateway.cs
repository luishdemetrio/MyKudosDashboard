using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IRecognitionGateway
{

    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();
}
