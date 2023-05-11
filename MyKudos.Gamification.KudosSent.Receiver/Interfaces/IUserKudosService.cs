
using MyKudos.Kudos.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

internal interface IUserKudosService
{

    public Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(string pUserId);
}
