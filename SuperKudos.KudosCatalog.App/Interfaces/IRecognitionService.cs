

using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.App.Interfaces;

public interface IRecognitionService
{
    IEnumerable<Recognition> GetRecognitions();

    bool SetRecognition(Recognition recognition);

    Task SeedDatabaseAsync();
}
