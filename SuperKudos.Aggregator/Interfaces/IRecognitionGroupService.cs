
using SuperKudos.Aggregator.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IRecognitionGroupService
{

    public Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups();

    public bool SetRecognitionGroups(RecognitionGroup group);

}
