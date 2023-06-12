
using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IRecognitionGroupService
{

    public Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups();

    public bool SetRecognitionGroups(RecognitionGroup group);

}
