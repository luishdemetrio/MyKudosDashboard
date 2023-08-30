
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.App.Services;

public sealed class RecognitionService : IRecognitionService
{

    private readonly IRecognitionRepository _recognitionRepository;

    public RecognitionService(IRecognitionRepository recognitionRepository)
    {
        _recognitionRepository = recognitionRepository;
    }

    public IEnumerable<Domain.Models.Recognition> GetRecognitions()
    {
        return _recognitionRepository.GetRecognitions();
    }

    public async Task SeedDatabaseAsync()
    {
        await _recognitionRepository.SeedDatabaseAsync().ConfigureAwait(false);
    }

    public bool SetRecognition(Recognition recognition)
    {
       return _recognitionRepository.SetRecognition(recognition);
    }
}
