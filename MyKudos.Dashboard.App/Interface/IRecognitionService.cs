
using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.App.Interface;

public interface IRecognitionService
{
    IEnumerable<Recognition> GetRecognitions();
}
