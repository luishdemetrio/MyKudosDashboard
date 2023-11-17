
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IRecognitionRepository
{
    IEnumerable<Recognition> GetRecognitions();

    Task SeedDatabaseAsync();

    bool SetRecognition(Recognition recognition);

    bool DeleteRecognition(int recognitionId);

    bool UpdateRecognition(Recognition recognition);
}
