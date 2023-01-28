using MyKudos.Recognition.Domain.Models;

namespace MyKudos.Recognition.Domain.Interfaces;

public interface IRecognitionRepository
{
    IEnumerable<MyKudos.Recognition.Domain.Models.Recognition> GetRecognitions();

    Task SeedDatabaseAsync();
}
