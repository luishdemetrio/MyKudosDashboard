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

    public bool Add(UserProfile user)
    {
        user.IsActive = true;
        _context.UserProfiles.Add(user);
        return _context.SaveChanges() > 0;
    }

    /// <summary>
    /// Add or update the users and deactivate the users that are not in the list
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public bool PopulateUserProfile(List<UserProfile> users)
    {
        try
        {

       
        // Set IsActive to false for all users in the database
        _context.Database.ExecuteSqlRaw("UPDATE UserProfiles SET IsActive = 0");

        foreach (var user in users)
        {
            var existingUser = _context.UserProfiles.FirstOrDefault(u => u.UserProfileId == user.UserProfileId);
            if (existingUser != null)
            {
                // Update existing user
                _context.Entry(existingUser).CurrentValues.SetValues(user);
                existingUser.IsActive = true;
            }
            else
            {
                // Add new user
                user.IsActive = true;
                _context.UserProfiles.Add(user);
            }
        }
        return _context.SaveChanges() > 0;
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Add or update the user
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public bool AddUpdateUserProfile(UserProfile user)
    {

        
        try
        {
            var existingUser = _context.UserProfiles.FirstOrDefault(u => u.UserProfileId == user.UserProfileId);

            if (existingUser != null)
            {
                // Update existing user
                _context.Entry(existingUser).CurrentValues.SetValues(user);
                existingUser.IsActive = true;
            }
            else
            {
                // Add new user
                user.IsActive = true;
                _context.UserProfiles.Add(user);
            }

            return _context.SaveChanges() > 0;
        }
        catch (Exception)
        {

            throw;
        }
       
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
            .Where(u => u.DisplayName.Contains(name) && u.IsActive) // EF.Functions.Like(u.DisplayName, $"%{name}%"))
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
