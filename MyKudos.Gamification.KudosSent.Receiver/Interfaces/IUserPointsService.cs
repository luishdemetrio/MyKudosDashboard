using MyKudos.Kudos.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IUserPointsService
{
    Task<UserPointScore> GetUserScore(string pUserId);

    
}
