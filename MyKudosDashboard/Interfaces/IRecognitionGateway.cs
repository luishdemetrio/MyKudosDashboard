using SuperKudos.Aggregator.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IRecognitionGateway
{

    Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups();

    Task<IEnumerable<Recognition>> GetRecognitionsAsync();
}
