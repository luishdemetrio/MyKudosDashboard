
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IRestServiceToken
{
    Task<string> GetAccessTokenAsync();
}
