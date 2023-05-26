using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IRecognitionService
{
    Task<IEnumerable<Recognition>> GetRecognitionsAsync();
}