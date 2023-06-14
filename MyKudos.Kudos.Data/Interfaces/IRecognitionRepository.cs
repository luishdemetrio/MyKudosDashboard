
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Interfaces;

public interface IRecognitionRepository
{
    IEnumerable<Recognition> GetRecognitions();

    Task SeedDatabaseAsync();

    bool SetRecognition(Recognition recognition);
}
