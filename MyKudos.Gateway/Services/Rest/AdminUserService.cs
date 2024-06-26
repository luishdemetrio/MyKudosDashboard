﻿using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Services.Rest;

public class AdminUserService : IAdminUserService
{
    private readonly string _serviceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<AdminUserService> _logger;

    public AdminUserService(IConfiguration config, IRestClientHelper clientHelper,
                            ILogger<AdminUserService> log)
    {
        _serviceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<bool> Add(Guid userProfileId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Guid, bool>(
                                $"{_serviceUrl}adminuser", 
                                HttpMethod.Post, 
                                userProfileId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Add: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> Delete(Guid userProfileId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Guid, bool>(
                                $"{_serviceUrl}adminuser",
                                HttpMethod.Delete,
                                userProfileId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Delete: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> IsAdminUser(Guid userProfileId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.GetApiData<bool>(
                                $"{_serviceUrl}adminuser/IsAdminUser/{userProfileId}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing IsAdminUser: {ex.Message}");
        }

        return result;
    }

    public async Task<IEnumerable<AdminUser>> GetAdminsUsers()
    {
        IEnumerable<AdminUser> result = null;

        try
        {
            result = await _restClientHelper.GetApiData<IEnumerable<AdminUser>>(
                                $"{_serviceUrl}adminuser/getadmins");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing IsAdminUser: {ex.Message}");
        }

        return result;
    }
}
