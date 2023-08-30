using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.App.Services;

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
