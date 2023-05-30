using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

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
        return _repository.GetTopUserScores(top);
    }

    public UserPointScore GetUserScore(string pUserId)
    {
        return _repository.GetUserScore(pUserId);   
    }
}
