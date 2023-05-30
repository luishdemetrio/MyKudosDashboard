using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Services;

public class RecognitionGroupService : IRecognitionGroupService
{

    private readonly IRecognitionGroupRepository _recognitionRepository;

    public RecognitionGroupService(IRecognitionGroupRepository recognitionRepository)
    {
        _recognitionRepository = recognitionRepository;
    }

    public IEnumerable<RecognitionGroup> GetRecognitionGroups()
    {
        return _recognitionRepository.GetRecognitionGroups();
    }

    public bool SetRecognitionGroups(RecognitionGroup group)
    {
        return _recognitionRepository.SetRecognitionGroups(group);
    }
}
