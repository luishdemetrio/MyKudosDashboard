using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IScorePointsRepository
{
    IEnumerable<ScorePoints> GetScore();

    bool UpdateScore(ScorePoints score);
}
