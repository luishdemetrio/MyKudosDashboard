
namespace MyKudos.Kudos.Token.Interfaces;

public interface IRestServiceToken
{
    Task<string> GetAccessTokenAsync();
}