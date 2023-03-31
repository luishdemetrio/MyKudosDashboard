
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IKudosService
{
    public Guid Send(KudosLog kudos);

    public IEnumerable<KudosLog> GetKudos();

    public int SendLike(string kudosId, string personId);

    string SendComments(Comments comment);

    IEnumerable<Comments> GetComments(string kudosId);

    bool UpdateComments(Comments comments);

    bool DeleteComments(string kudosId, Guid commentId);
}
