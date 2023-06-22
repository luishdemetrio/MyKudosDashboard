using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.App.Services;

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
