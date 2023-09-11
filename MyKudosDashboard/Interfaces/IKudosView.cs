using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosView
{

    UserProfile User { get; set; }

    bool IsLikeDisabled { get; set; }

    bool Filled(KudosResponse item);

    void LikeKudosClick(KudosResponse item);
}
