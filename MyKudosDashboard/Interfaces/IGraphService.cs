using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces
{
    public interface IGraphService
    {
        Task<string> GetAppOnlyTokenAsync();
        Task<string> GetUserPhoto(string userid);
        //Task<GraphUserPhotos> GetUserPhotos(GraphUsersDTO users);
        //Task<GraphUsersDTO> GetUsers(string name);
    }
}