using System.Threading.Tasks;

namespace MyKudos.Kudos.Notification.Receiver.Interfaces;

public interface IRestServiceToken
{
    Task<string> GetAccessTokenAsync();
}
