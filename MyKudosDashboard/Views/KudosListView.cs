using Microsoft.Graph;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Common;
using MyKudosDashboard.Interfaces;
using System.Collections.Concurrent;
using System.Text;

namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{

    private readonly IKudosGateway _gatewayService;

    public ConcurrentDictionary<int, KudosResponse> KudosList { get; set; }

    private bool _resetKudos = false;

    public KudosListView(IKudosGateway gatewayService)
    {
        _gatewayService = gatewayService;

        LoadKudoListAgain();

    }

    private async void LoadKudoListAgain()
    {
        _resetKudos = true;

        if ( await LoadKudos(0, null, KudosCommonVariables.KudosTypeTab))
            _resetKudos = false;
    }

    public async Task<bool> LoadKudos(int pageNumber, Guid? UserProfileId, KudosTypeTab kudosTypeTab)
    {

        IEnumerable<KudosResponse> kudos = null;

        //this happens when user clicks on visualize just my team
        if ( KudosList is null || pageNumber == 1)
            KudosList = new();

        switch (kudosTypeTab)
        {
            case KudosTypeTab.All:
                kudos = await GetKudos(pageNumber).ConfigureAwait(true);
                break;

            case KudosTypeTab.FromMe:

                kudos = await GetKudosFromMe(UserProfileId.ToString(),
                                             pageNumber
                                             ).ConfigureAwait(true);
                break;

            case KudosTypeTab.ToMe:

                kudos = await GetKudosToMe(UserProfileId.ToString(),
                                           pageNumber
                                           ).ConfigureAwait(true);
                break;
        }

        /// transform the list in a dictionary
        foreach (var kudo in kudos)
        {
            if (!KudosList.ContainsKey(kudo.Id))
            {
                KudosList.TryAdd(kudo.Id, kudo);
            }
        }

        return (kudos != null & kudos.Count() > 0);
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

    public async Task<bool> UpdateKudos(KudosMessage kudos)
    {
       return await _gatewayService.UpdateKudos(kudos);
    }

    public async Task<bool> DeleteKudos(int kudosId)
    {
        bool result =  await _gatewayService.DeleteKudos(kudosId);

        if (KudosList.ContainsKey(kudosId))
        {
            result = KudosList.TryRemove(kudosId, out KudosResponse kudos);

        }

        return result;

    }

    public string ExportToCsv()
    {

        var sb = new StringBuilder();
        sb.AppendLine("Title,Message,SendOn,NumberOfLikes,NumberOfComments,From,Receivers");
        foreach (var item in KudosList)
        {
            var receivers = string.Join(";", item.Value.Receivers.Select(r => r.Name));
            sb.AppendLine($"{item.Value.Title},{item.Value.Message.Replace("\n", "")}," +
                $"{item.Value.SendOn},{item.Value.Likes.Count}," +
                $"{item.Value.Comments.Count},{item.Value.From.Name},{receivers}");
        }

        return sb.ToString();

    }

    /// <summary>
    /// This method is used to delete locally the Kudos, as it is called by an event hub callback.
    /// </summary>
    /// <param name="kudosId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool DeleteKudosLocally(int kudosId)
    {
        bool result = false;

        if (KudosList.ContainsKey(kudosId))
        {
            result = KudosList.TryRemove(kudosId, out KudosResponse kudos);

        }
        return result;
    }

    public bool KudosMessageUpdatedLocally(KudosMessage message)
    {
        bool result = false;

        if (KudosList.ContainsKey(message.KudosId))
        {
            KudosList.TryGetValue(message.KudosId, out KudosResponse kudos);

            kudos.Message = message.Message;

            result = true;

        }

        return result;
    }

    public bool UpdateLikesCountAndToolTipLocally(LikeGateway pLike)
    {
        return UpdateLikesCountAndToolTipLocally(pLike, true);
    }

    public bool UpdateUnLikesCountAndToolTipLocally(LikeGateway pLike)
    {
        return UpdateLikesCountAndToolTipLocally(pLike, false);
    }


    private bool UpdateLikesCountAndToolTipLocally(LikeGateway pLike, bool isLike)
    {
        bool result = false;

        if (KudosList.ContainsKey(pLike.KudosId))
        {
            var kudo = KudosList[pLike.KudosId];
            var like = kudo.Likes.Where(l => l.Id == pLike.FromPerson.Id).FirstOrDefault();

            if (isLike)
            {
                if (like == null)
                {
                    kudo.Likes.Add(new MyKudos.Gateway.Domain.Models.Person()
                    {
                        Id = pLike.FromPerson.Id,
                        Name = pLike.FromPerson.Name,
                        Photo = pLike.FromPerson.Photo
                    });
                    result = true;
                }
            }
            else
            {
                if (like != null)
                {
                    kudo.Likes.Remove(like);
                    result = true;
                }
                else
                {
                    //force to reload
                    LoadKudoListAgain();
                }
            }
        }
        else
        {
            //force to reload
            LoadKudoListAgain();
        }

        return result;
    }

    public bool UpdateKudosLocally(KudosResponse pKudos)
    {
        bool result = false;

        if (!KudosList.ContainsKey(pKudos.Id))
            result = KudosList.TryAdd(pKudos.Id, pKudos);
        
        return result;
    }

    public bool CommentsDeletedLocaly(CommentsRequest pComments)
    {
        bool result = false;

        if (KudosList.ContainsKey(pComments.KudosId))
        {
            var kudo = KudosList[pComments.KudosId];

            var comments = kudo.Comments.Where(k => k == pComments.Id).FirstOrDefault();

            if (kudo.Comments.Contains(comments))
            {
                kudo.Comments.Remove(comments);
                result = true;
            }
        }

        return result;
    }

    public bool CommentsSentLocally(CommentsRequest pComments)
    {
        bool result = false;

        if (KudosList.ContainsKey(pComments.KudosId))
        {
            var kudo = KudosList[pComments.KudosId];

            if (!kudo.Comments.Contains(pComments.Id))
            {
                kudo.Comments.Add(pComments.Id);

                result = true;
            }
        }

        return result;
    }
}
