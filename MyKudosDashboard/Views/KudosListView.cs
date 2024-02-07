using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Common;
using MyKudosDashboard.Components;
using MyKudosDashboard.Interfaces;
using System.Collections.Concurrent;

namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{

    private readonly IKudosGateway _gatewayService;

    public ConcurrentDictionary<int, KudosResponse> KudosList { get; set; }

    public KudosListView(IKudosGateway gatewayService)
    {
        _gatewayService = gatewayService;

        LoadKudos();

    }

    private async void LoadKudos(bool resetCollection=false)
    {
        //this happens when user clicks on visualize just my team
        if (resetCollection)
            KudosList = new();
        
        var kudos = await LoadKudos(0, null, KudosTypeTab.All);

        /// transform the list in a dictionary
        foreach (var kudo in kudos)
        {
            if (!KudosList.ContainsKey(kudo.Id))
            {
                KudosList.TryAdd(kudo.Id, kudo);
            }
        }
    }

    public async Task<bool> SendLikeAsync(int pKudosId, Guid pFromPersonId)
    {
        return await _gatewayService.Like(new SendLikeGateway(pKudosId, pFromPersonId));
    }

    public async Task<bool> SendUndoLikeAsync(int pKudosId, Guid pFromPersonId)
    {
        return await _gatewayService.UndoLike(new SendLikeGateway(pKudosId, pFromPersonId));
    }

   public async Task<IEnumerable<KudosResponse>> LoadKudos(int pageNumber, Guid? UserProfileId, KudosTypeTab kudosTypeTab)
    {
        
        IEnumerable<KudosResponse> kudos = null;

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
        
        return kudos;
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
            KudosList.TryRemove(kudosId, out KudosResponse kudos);

        }

        return result;

    }

    public void ExportToCsv()
    {
        throw new NotImplementedException();
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
                    kudo.Likes.Add(new Person()
                    {
                        Id = pLike.FromPerson.Id,
                        Name = pLike.FromPerson.Name,
                        Photo = pLike.FromPerson.Photo
                    });
                }
            }
            else
            {
                if (like != null)
                {
                    kudo.Likes.Remove(like);
                }
                else
                {
                    //force to reload
                    LoadKudos(true);
                }
            }
        }
        else
        {
            //force to reload
            LoadKudos(true);
        }

        return result;
    }
}
