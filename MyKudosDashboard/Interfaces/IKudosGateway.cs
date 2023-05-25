﻿using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosGateway
{
    

    Task<string> SendKudos(KudosRequest kudos);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);


    Task<bool> Like(LikeGateway like);

    Task<bool> UndoLike(LikeGateway like);


}
