﻿using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IGamificationService
{

    Task<UserScore> GetUserScoreAsync(string pUserId);    

}
