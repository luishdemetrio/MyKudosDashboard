﻿
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Services;

public class UserScoreService : IUserScoreService
{

    private readonly IUserScoreRepository _userScoreRepository;


    public UserScoreService(IUserScoreRepository userScoreRepository)
    {
        _userScoreRepository = userScoreRepository;
    }


    public UserScore GetUserScore(string pUserId)
    {
        return _userScoreRepository.GetUserScore(pUserId);
    }

    public UserScore? SetUserScore(UserScore userScore)
    {

        return _userScoreRepository.SetUserScore(userScore);

    }

    public IEnumerable<UserScore> GetTopUserScores(int top)
    {
        return _userScoreRepository.GetTopUserScores(top);
    }

    public bool UpdateGroupScore(UserScore userScore)
    {
        return _userScoreRepository.UpdateGroupScore(userScore);
    }
}