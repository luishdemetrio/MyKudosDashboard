﻿using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Common;
using System.Collections.Concurrent;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{
    ConcurrentDictionary<int, KudosResponse> KudosList { get; set; }

    Task<bool> SendLikeAsync(int pKudosId, Guid pFromPersonId);

    Task<bool> SendUndoLikeAsync(int pKudosId, Guid pFromPersonId);

    Task<IEnumerable<KudosResponse>> LoadKudos(int pageNumber, Guid? UserProfileId, 
                                               KudosTypeTab kudosTypeTab);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber);
    
    Task<bool> UpdateKudos(KudosMessage kudos);

    Task<bool> DeleteKudos(int kudosId);

    bool DeleteKudosLocally(int kudosId);

    bool KudosMessageUpdatedLocally(KudosMessage message);

    bool UpdateLikesCountAndToolTipLocally(LikeGateway pLike);
    bool UpdateUnLikesCountAndToolTipLocally(LikeGateway pLike);

    void ExportToCsv();
}
