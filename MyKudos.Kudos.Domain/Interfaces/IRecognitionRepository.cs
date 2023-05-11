
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IRecognitionRepository
{
    IEnumerable<Recognition> GetRecognitions();

    Task SeedDatabaseAsync();
}
