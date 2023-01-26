namespace MyKudos.Gateway.Interfaces
{
    public interface IKudosService
    {
        IEnumerable<Models.Kudos> GetKudos();

        bool Send(Models.KudosRequest kudos);

        bool SendLike(string kudosId, string personId);
    }
}