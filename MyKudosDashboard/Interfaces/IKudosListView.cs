﻿using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{

    Task<bool> SendLikeAsync(Like like);




    
}
