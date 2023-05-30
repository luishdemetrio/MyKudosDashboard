using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IRecognitionGateway
{

    Task<IEnumerable<Recognition>> GetRecognitionsAsync();
}
