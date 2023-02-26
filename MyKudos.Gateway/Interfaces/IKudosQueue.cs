﻿using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosQueue
{
    Task SendKudosAsync(KudosNotification kudos);

    Task SendLikeAsync(LikeGateway like );
}