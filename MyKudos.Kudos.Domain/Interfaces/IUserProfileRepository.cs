﻿using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IUserProfileRepository
{
  
    bool Add(UserProfile user);
    bool AddRange(List<UserProfile> user);

    List<UserProfile> GetAll();
   
}
