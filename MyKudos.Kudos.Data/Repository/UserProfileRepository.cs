using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Repository;

public class UserProfileRepository : IUserProfileRepository
{

    private KudosDbContext _context;

    public UserProfileRepository(KudosDbContext kudosLikeDbContext)
    {
        _context = kudosLikeDbContext;
    }

    public bool Truncate()
    {
         return _context.Database.ExecuteSqlRaw("TRUNCATE TABLE UserProfiles") >0;

    }

    public bool Add(UserProfile user)
    {
        _context.UserProfiles.Add(user);
        return _context.SaveChanges() > 0;
    }

    public bool AddRange(List<UserProfile> user)
    {
        _context.UserProfiles.AddRange(user);
        return _context.SaveChanges() > 0;
    }

    public bool PopulateUserProfile(List<UserProfile> users)
    {
        bool result = false;

        //        Truncate();
        _context.UserProfiles.ExecuteDelete();
        result = AddRange(users);
        
        return result;
    }

    public List<UserProfile> GetAll()
    {
        return _context.UserProfiles.ToList();
    }

    public string? GetUserPhoto(Guid userid)
    {
        return _context.UserProfiles.Where(u => u.UserProfileId == userid).Select(s=> s.Photo96x96).First();
    }

    public List<UserProfile> GetUsers(string name)
    {
        return _context.UserProfiles
            .Where(u => u.DisplayName.Contains(name)) // EF.Functions.Like(u.DisplayName, $"%{name}%"))
            .ToList();
    }

    public List<UserProfile> GetUsers(Guid[] ids)
    {
        return _context.UserProfiles.Where(user => ids.Contains(user.UserProfileId))
            .ToList();
    }

    /// <summary>
    /// Returns the user info and if the user has direct reports
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public UserProfile? GetUser(Guid userId)
    {
        var user =  _context.UserProfiles.Where(user => user.UserProfileId == userId).FirstOrDefault();

        if (user != null)
        {
            user.HasDirectReports = _context.UserProfiles.Any(up => up.ManagerId == userId);
        }

        return user;
    }
}
