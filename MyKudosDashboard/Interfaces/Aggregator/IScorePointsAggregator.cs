using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces.Aggregator;

public interface IScorePointsAggregator
{
    Task<Points> GetPoints();

    Task<bool> UpdateScore(Points points);
}
