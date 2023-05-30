

using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IRecognitionService
{
    IEnumerable<Recognition> GetRecognitions();

    bool SetRecognition(Recognition recognition);

    Task SeedDatabaseAsync();
}
