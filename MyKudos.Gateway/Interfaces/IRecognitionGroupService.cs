
using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IRecognitionGroupService
{

    public Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups();

    public Task<bool> AddNewRecognitionGroup(RecognitionGroup group);

    Task<bool> DeleteRecognitionGroup(int recognitionGroupId);

    Task<bool> UpdateRecognitionGroup(RecognitionGroup group);

}
