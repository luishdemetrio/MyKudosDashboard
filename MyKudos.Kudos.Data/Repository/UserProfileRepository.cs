﻿using MyKudos.Kudos.Data.Context;
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
        _context.UserProfiles.Add(user);
        return _context.SaveChanges() > 0;
    }

    public bool AddRange(List<UserProfile> user)
    {
        _context.UserProfiles.AddRange(user);
        return _context.SaveChanges() > 0;
    }

    public List<UserProfile> GetAll()
    {
        return _context.UserProfiles.ToList();
    }

    public List<UserProfile> GetUsers(string name)
    {
        return _context.UserProfiles.Where(u => u.DisplayName.Contains(name)).ToList();
    }
}
