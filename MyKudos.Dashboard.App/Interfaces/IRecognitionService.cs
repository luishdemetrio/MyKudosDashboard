
using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.App.Interfaces;

public interface IRecognitionService
{
    IEnumerable<Recognition> GetRecognitions();
}
