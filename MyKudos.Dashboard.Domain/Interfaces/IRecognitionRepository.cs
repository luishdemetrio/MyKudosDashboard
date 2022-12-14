using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.Domain.Interfaces;

public interface IRecognitionRepository
{
    IEnumerable<Recognition> GetRecognitions();
}
