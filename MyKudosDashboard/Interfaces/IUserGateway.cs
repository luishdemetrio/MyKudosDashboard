﻿using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IUserGateway
{
    Task<IEnumerable<UserViewModel>> GetUsers(string name);

    Task<string> GetUserPhoto(string userid);

  
}