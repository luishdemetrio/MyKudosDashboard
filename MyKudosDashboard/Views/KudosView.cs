//using MyKudos.Gateway.Domain.Models;
//using MyKudosDashboard.Interfaces;

//namespace MyKudosDashboard.Views;


//public class KudosView : IKudosView
//{

    
//    private readonly object _lock = new object();

//    public UserProfile User { get ; set ; }
//    public bool IsLikeDisabled { get ; set ; }

//    public bool Filled(KudosResponse item)
//    {
//        bool filled = false;

//        if(User != null)
//        {
//            filled = item.Likes.Any(l => l.Id == User.UserProfileId);
//        }

//        return filled;
//    }

//    public void LikeKudosClick(KudosResponse item)
//    {

//        lock (_lock)
//        {
//            if (IsLikeDisabled)
//                return;

//            //disable button to avoid user click twice
//            IsLikeDisabled = true;
//        }

        
//            // I am populating the object LikeGateway to add the Like locally for the current user,
//            // for better performance.
//            // To call the API to update the like we just need the KudosId and UserProfileId

//            var requestLike = new LikeGateway
//                                (
//                                    KudosId: item.Id,
//                                    FromPerson: new Person
//                                    {
//                                        Id = User.UserProfileId,
//                                        Name = User.DisplayName,
//                                        Photo = User.Photo96x96
//                                    }
//                                );



//        if (KudosItem.Likes.Any(l => l.Id == User.UserProfileId))
//        {

//            SendUndoLikeCallBack?.Invoke(item.Id, User.UserProfileId);

//            UpdateLikesAsync(requestLike, false); // To remove a like

//        }
//        else
//        {

//            SendLikeCallBack?.Invoke(item.Id, User.UserProfileId);

//            UpdateLikesAsync(requestLike, true); // To add a like
//        }

        

//    }
//}
