using MyKudos.Gateway.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Helpers;

public class KudosHelper
{
    public static IEnumerable<KudosResponse> GetKudos(IEnumerable<Kudos.Domain.Models.Kudos> kudos, 
                                                      string defaultProfilePicture,
                                                      bool userSmallPhoto = false)
    {
        var result = new List<KudosResponse>();

        foreach (var kudo in kudos)
        {

            

            var kudosResponse = new KudosResponse()
            {
                Id = kudo.KudosId,
                From = new GatewayDomain.Person()
                {
                    Id = kudo.UserFrom.UserProfileId,
                    Name = kudo.UserFrom.DisplayName,
                    Photo = kudo.UserFrom.Photo != null ? $"data:image/png;base64,{kudo.UserFrom.Photo}" : defaultProfilePicture,
                    EMail = kudo.UserFrom.Mail
                },
                Likes = kudo.Likes.Where(l => l.Person != null).Select(x => new GatewayDomain.Person()
                {
                    Id = x.Person.UserProfileId,
                    Name = x.Person.DisplayName,
                    Photo = x.Person.Photo != null ? $"data:image/png;base64,{x.Person.Photo}" : defaultProfilePicture
                }).ToList(),
                Comments = (kudo.Comments is null) ? new List<int>() : kudo.Comments.Select(c => c.CommentsId).ToList(),
                Message = kudo.Message,
                SendOn = kudo.Date,
                Title = kudo.Recognition.Title
                

            };

            foreach (var receiver in kudo.Recognized)
            {
                var ToPhoto = userSmallPhoto ? receiver.Person.Photo : receiver.Person.Photo96x96;

                kudosResponse.Receivers.Add(new GatewayDomain.Person()
                {
                    Id = receiver.Person.UserProfileId,
                    Name = receiver.Person.DisplayName,
                    Photo = ToPhoto != null ? $"data:image/png;base64,{ToPhoto}" : defaultProfilePicture,
                    GivenName = receiver.Person.GivenName,
                    EMail = kudo.UserFrom.Mail
                });
            }


            result.Add(kudosResponse);


        }

        return result;
    }
}
