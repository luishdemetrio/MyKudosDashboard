
namespace MyKudos.Communication.Helper.Interfaces;

public interface IRestServiceToken
{
    Task<string> GetAccessTokenAsync();
}