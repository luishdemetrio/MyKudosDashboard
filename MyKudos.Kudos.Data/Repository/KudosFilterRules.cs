

namespace MyKudos.Kudos.Data.Repository;

internal static class KudosFilterRules
{

    ////It is filtered to include only kudos where the manager of the user who gave the kudos 
    ///or the manager of any recognized user is the same as the manager id.
    ///Additionally, if the manager of the user who gave the kudos is not specified, 
    ///the kudos will be included if the user profile ID of the user who gave the kudos is the same as the manager id.


    public static IQueryable<Domain.Models.Kudos> FilterKudosByManagerOrProfile(IQueryable<Domain.Models.Kudos> kudosQuery, 
                                                                                Guid? userId)
    {
        return kudosQuery.Where(k => (k.UserFrom.ManagerId == userId || 
                                      k.Recognized.Any(u => u.Person.ManagerId == userId)
                                     ) || 
                                      (k.UserFrom.ManagerId.HasValue == false && 
                                       k.UserFrom.UserProfileId == userId)
                               );
    }

}
