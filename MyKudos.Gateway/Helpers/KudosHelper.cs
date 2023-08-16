using MyKudos.Gateway.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Helpers;

public class KudosHelper
{
    public static IEnumerable<KudosResponse> GetKudos(IEnumerable<Kudos.Domain.Models.Kudos> kudos, string defaultProfilePicture)
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
                    Photo = kudo.UserFrom.Photo != null ? $"data:image/png;base64,{kudo.UserFrom.Photo}" : defaultProfilePicture
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
                kudosResponse.Receivers.Add(new GatewayDomain.Person()
                {
                    Id = receiver.Person.UserProfileId,
                    Name = receiver.Person.DisplayName,
                    Photo = receiver.Person.Photo96x96 != null ? $"data:image/png;base64,{receiver.Person.Photo96x96}" : defaultProfilePicture,
                    GivenName = receiver.Person.GivenName
                });
            }


            result.Add(kudosResponse);


        }

        return result;
    }
}
