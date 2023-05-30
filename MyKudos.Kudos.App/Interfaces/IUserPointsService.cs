using MyKudos.Kudos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKudos.Kudos.App.Interfaces;

public interface IUserPointsService
{

    List<UserPoint> GetTopUserScores(int top);

    UserPointScore GetUserScore(string pUserId);
}
