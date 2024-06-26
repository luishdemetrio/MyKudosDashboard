﻿using MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IAdminUserService
{
    Task<bool> Add(Guid userProfileId);
    Task<bool> Delete(Guid userProfileId);
    Task<bool> IsAdminUser(Guid userProfileId);
    Task<IEnumerable<AdminUser>> GetAdminsUsers();
}