using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Common;
using System.Collections.Concurrent;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{
    ConcurrentDictionary<int, KudosResponse> KudosList { get; set; }

    Task<bool> SendLikeAsync(int pKudosId, Guid pFromPersonId);

    Task<bool> SendUndoLikeAsync(int pKudosId, Guid pFromPersonId);

    Task<bool> LoadKudos(int pageNumber, KudosTypeTab kudosTypeTab);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMe(int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosFromMe(int pageNumber);
    
    Task<bool> UpdateKudos(KudosMessage kudos);

    Task<bool> DeleteKudos(int kudosId);

    bool DeleteKudosLocally(int kudosId);

    bool KudosMessageUpdatedLocally(KudosMessage message);

    bool UpdateLikesCountAndToolTipLocally(LikeGateway pLike);
    bool UpdateUnLikesCountAndToolTipLocally(LikeGateway pLike);

    bool UpdateKudosLocally(KudosResponse pKudos);

    bool CommentsDeletedLocaly(CommentsRequest pComments);

    bool CommentsSentLocally(CommentsRequest pComments);

    string ExportToCsv();
}
