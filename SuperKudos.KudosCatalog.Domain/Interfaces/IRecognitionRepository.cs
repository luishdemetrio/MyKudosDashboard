
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.Domain.Interfaces;

public interface IRecognitionRepository
{
    IEnumerable<Recognition> GetRecognitions();

    Task SeedDatabaseAsync();

    bool SetRecognition(Recognition recognition);
}
