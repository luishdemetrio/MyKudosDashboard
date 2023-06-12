using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IRecognitionGateway
{

    Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups();

    Task<IEnumerable<Recognition>> GetRecognitionsAsync();
}
