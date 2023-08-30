using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.App.Services;

public class UserPointsService : IUserPointsService
{

    private readonly IUserPointsRepository _repository;


    public UserPointsService(IUserPointsRepository repository)
    {
        _repository = repository;
    }


    public List<UserPoint> GetTopUserScores(int top)
    {
        return _repository.GetTopUserScores(top).Where(t => t.TotalPoints >0).ToList();
    }

    public UserPointScore GetUserScore(Guid pUserId)
    {
        return _repository.GetUserScore(pUserId);   
    }
}
