using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IScorePointsService
{
    Task<ScorePoints> GetScore();

    Task<bool> UpdateScore(ScorePoints scorePoints);
}
