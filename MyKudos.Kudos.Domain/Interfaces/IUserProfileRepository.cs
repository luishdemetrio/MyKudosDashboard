﻿using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IUserProfileRepository
{
  
    bool Add(UserProfile user);
    
    List<UserProfile> GetAll();

    List<UserProfile> GetUsers(string name);

    string? GetUserPhoto(Guid userid);

    bool PopulateUserProfile(List<UserProfile> users);
    bool AddUpdateUserProfile(UserProfile user);

    List<UserProfile> GetUsers(Guid[] ids);

    UserProfile? GetUser(Guid userId);
}
