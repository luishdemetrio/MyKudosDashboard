using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces.Aggregator;

public interface IRecognitionGroupAggregator
{
    public Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups();

    public Task<bool> AddNewRecognitionGroup(RecognitionGroup group);

    Task<bool> DeleteRecognitionGroup(int recognitionGroupId);

    Task<bool> UpdateRecognitionGroup(RecognitionGroup group);
}
