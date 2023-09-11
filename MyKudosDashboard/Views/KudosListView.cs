using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;


namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{

    private readonly IKudosGateway _gatewayService;


    public KudosListView(IKudosGateway gatewayService)
    {
        _gatewayService = gatewayService;

    }

    public async Task<bool> SendLikeAsync(int pKudosId, Guid pFromPersonId)
    {
        return await _gatewayService.Like(new SendLikeGateway(pKudosId, pFromPersonId));
    }

    public async Task<bool> SendUndoLikeAsync(int pKudosId, Guid pFromPersonId)
    {
        return await _gatewayService.UndoLike(new SendLikeGateway(pKudosId, pFromPersonId));
    }

   

    public async Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber)
    {
      
        return await _gatewayService.GetKudos(pageNumber);
           
    }

    public async Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber)
    {
        return await _gatewayService.GetKudosToMe(userId, pageNumber);
    }

    public async Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber)
    {
        return await _gatewayService.GetKudosFromMe(userId, pageNumber);
    }


    public async Task<IEnumerable<KudosResponse>> GetKudosToMyDirectReports(string userId, int pageNumber)
    {
        return await _gatewayService.GetKudosToMyDirects(userId, pageNumber);
    }

    public async Task<bool> UpdateKudos(KudosMessage kudos)
    {
       return await _gatewayService.UpdateKudos(kudos);
    }

    public async Task<bool> DeleteKudos(int kudosId)
    {
        return await _gatewayService.DeleteKudos(kudosId);
    }
}
