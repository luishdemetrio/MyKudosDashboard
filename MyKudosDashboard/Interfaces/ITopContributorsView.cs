using SuperKudos.Aggregator.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface ITopContributorsView
{
    void RegisterObserver(string userId);
    void UnregisterObserver(string userId);

    public delegate void UpdateTopContributorsCallBack();

    public UpdateTopContributorsCallBack TopContributorsCallBack { get; set; }

    Task<IEnumerable<TopContributors>> GetTopContributors();
}
