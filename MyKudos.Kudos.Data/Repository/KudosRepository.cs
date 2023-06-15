﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.Data.Repository;

public class KudosRepository : IKudosRepository
{

	private KudosDbContext _kudosDbContext;
	
	private int _maxPageSize;

	public KudosRepository(KudosDbContext kudosDbContext, IConfiguration configuration)
	{
		_kudosDbContext = kudosDbContext;

		

		_maxPageSize = int.Parse(configuration["KudosMaxPageSize"]);

	}

	public int Add(Domain.Models.Kudos kudos)
	{
		_kudosDbContext.Kudos.Add(kudos);
		_kudosDbContext.SaveChanges();

		return kudos.KudosId;
	}




	public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize=5)
	{

		if(pageSize > _maxPageSize)
		{
			pageSize = _maxPageSize;
		}

		return await _kudosDbContext.Kudos.OrderByDescending(k => k.Date)
					.Skip(pageSize * (pageNumber - 1))
					.Take(pageSize)					
					.Include(l=> l.Likes)
					.Include(c=> c.Comments)
                    .Include(u => u.UserFrom)
                    .Include(u => u.UserTo)
                    .Include(u => u.Recognition)
                    .ToListAsync();
	}


    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5)
    {

        if (pageSize > _maxPageSize)
        {
            pageSize = _maxPageSize;
        }

        return await _kudosDbContext.Kudos
					.Where(k => k.FromPersonId == pUserId)
					.OrderByDescending(k => k.Date)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .Include(l => l.Likes)
                    .Include(c => c.Comments)
                    .Include(u => u.UserFrom)
                    .Include(u => u.UserTo)
                    .Include(u => u.Recognition)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5)
    {

        if (pageSize > _maxPageSize)
        {
            pageSize = _maxPageSize;
        }

        return await _kudosDbContext.Kudos
                    .Where(k => k.ToPersonId == pUserId)
                    .OrderByDescending(k => k.Date)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .Include(l => l.Likes)
                    .Include(c => c.Comments)
                     .Include(u => u.UserFrom)
                    .Include(u => u.UserTo)
                    .Include(u => u.Recognition)
                    .ToListAsync();
    }


    //this method is used to calculate the badge
    public IEnumerable<Domain.Models.Kudos> GetUserKudos(Guid pUserId)
    {

        return _kudosDbContext.Kudos.Where(k => k.ToPersonId == pUserId);

    }





}
