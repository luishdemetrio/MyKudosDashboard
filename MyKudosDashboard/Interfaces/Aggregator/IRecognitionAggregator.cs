using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces.Aggregator;

public interface IRecognitionAggregator
{
    public Task<IEnumerable<Recognition>> GetRecognitions();

    public Task<bool> AddNewRecognition(Recognition group);

    Task<bool> DeleteRecognition(int recognitionId);

    Task<bool> UpdateRecognition(Recognition group);
}
