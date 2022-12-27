namespace MyKudos.Gateway.Interfaces
{
    public interface IRecognitionService
    {
        IEnumerable<Models.Recognition> GetRecognitions();
    }
}