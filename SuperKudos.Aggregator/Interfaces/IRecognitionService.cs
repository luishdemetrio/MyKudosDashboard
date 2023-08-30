using SuperKudos.Aggregator.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IRecognitionService
{
    Task<IEnumerable<Recognition>> GetRecognitionsAsync();
}