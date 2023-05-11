
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.App.Services;

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
}
