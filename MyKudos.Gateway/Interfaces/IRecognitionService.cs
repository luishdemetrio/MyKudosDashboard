using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IRecognitionService
{
    Task<IEnumerable<Recognition>> GetRecognitionsAsync();

    Task<bool> AddRecognition(Recognition recognition);

    Task<bool> DeleteRecognition(int recognitionId);

    Task<bool> UpdateRecognition(Recognition recognition);
}