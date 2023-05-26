using MyKudos.Kudos.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IUserScoreService
{
    Task<UserScore> GetUserScoreAsync(Guid pUserId);

    Task<UserScore> SetUserScoreAsync(UserScore userScore);
}
