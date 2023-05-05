﻿using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGamificationGateway
{

    Task<UserScore> GetUserScoreAsync(string pUserId);

    Task<IEnumerable<TopContributors>> GetTopContributors();
}