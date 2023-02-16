namespace MyKudos.Gateway.Interfaces;

public interface IRecognitionService
{
    Task<IEnumerable<Models.Recognition>> GetRecognitionsAsync();
}