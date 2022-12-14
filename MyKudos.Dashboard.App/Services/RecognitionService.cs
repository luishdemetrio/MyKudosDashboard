using MyKudos.Dashboard.App.Interface;
using MyKudos.Dashboard.Domain.Interfaces;
using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.App.Services;

public sealed class RecognitionService : IRecognitionService
{

    private readonly IRecognitionRepository _recognitionRepository;

    public RecognitionService(IRecognitionRepository recognitionRepository)
    {
        _recognitionRepository = recognitionRepository;
    }

    public IEnumerable<Recognition> GetRecognitions()
    {
        return _recognitionRepository.GetRecognitions();
    }
}
