using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface ITopContributorsView
{
    public delegate void UpdateTopContributorsCallBack();

    public UpdateTopContributorsCallBack TopContributorsCallBack { get; set; }

    Task<IEnumerable<TopContributors>> GetTopContributors();
}
