using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Data.Repository;

public class AdminUserRepository : IAdminUserRepository
{
    private KudosDbContext _context;

    public AdminUserRepository(KudosDbContext context)
    {
        _context = context;
    }

    public bool Add(Guid userProfileId)
    {
        _context.AdminUsers.Add(new AdminUser{ UserProfileId = userProfileId });

        return _context.SaveChanges() > 0;
    }

    public bool Delete(Guid userProfileId)
    {
        var admin = GetAdminUser(userProfileId);

        if (admin == null)
        {
            return false;
        }

        _context.AdminUsers.Remove(admin);

        return _context.SaveChanges() > 0;
    }

    public bool IsAdminUser(Guid userProfileId)
    {
        return GetAdminUser(userProfileId) != null;
    }


    private AdminUser? GetAdminUser(Guid userProfileId)
    {
        return _context.AdminUsers.SingleOrDefault(user => user.UserProfileId == userProfileId);
    }

}
