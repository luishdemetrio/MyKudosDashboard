
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

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

    public Domain.Models.Kudos? GetKudos(int kudosId)
    {

        var kudos =  _kudosDbContext.Kudos.Where(k => k.KudosId == kudosId)
                    .Include(l => l.Likes)
                    .Include(c => c.Comments)
                    .Include(u => u.UserFrom)
                    .Include(u => u.Recognized)
                    .Include(u => u.Recognition)
                    .First();



        foreach (var receiver in kudos.Recognized)
        {
            receiver.Person = _kudosDbContext.UserProfiles.First(u => u.UserProfileId == receiver.ToPersonId);
        }

        return kudos;
        
    }


    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize=5)
	{

		if(pageSize > _maxPageSize)
		{
			pageSize = _maxPageSize;
		}

		var kudos =  await _kudosDbContext.Kudos.OrderByDescending(k => k.Date)
					.Skip(pageSize * (pageNumber - 1))
					.Take(pageSize)					
					.Include(l=> l.Likes)
					.Include(c=> c.Comments)
                    .Include(u => u.UserFrom)
                    .Include(u => u.Recognized)
                    .Include(u => u.Recognition)
        .ToListAsync();


        foreach (var kudo in kudos)
        {
            foreach (var like in kudo.Likes)
            {
                like.Person = _kudosDbContext.UserProfiles.First(u => u.UserProfileId == like.PersonId);
            }

            foreach (var receiver in kudo.Recognized)
            {
                receiver.Person = _kudosDbContext.UserProfiles.First(u => u.UserProfileId == receiver.ToPersonId);
            }
        }

        return kudos;
    }


    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5)
    {

        if (pageSize > _maxPageSize)
        {
            pageSize = _maxPageSize;
        }

        var kudos =  await _kudosDbContext.Kudos
					.Where(k => k.FromPersonId == pUserId)
					.OrderByDescending(k => k.Date)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .Include(l => l.Likes)
                    .Include(c => c.Comments)
                    .Include(u => u.UserFrom)
                    .Include(u => u.Recognized)
                    .Include(u => u.Recognition)
                    .ToListAsync();

        foreach (var kudo in kudos)
        {
            foreach (var like in kudo.Likes)
            {
                like.Person = _kudosDbContext.UserProfiles.First(u => u.UserProfileId == like.PersonId);
            }

            foreach (var receiver in kudo.Recognized)
            {
                receiver.Person = _kudosDbContext.UserProfiles.First(u => u.UserProfileId == receiver.ToPersonId);
            }
        }

        return kudos;
    }

    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5)
    {

        if (pageSize > _maxPageSize)
        {
            pageSize = _maxPageSize;
        }

        var kudos =  await _kudosDbContext.Kudos
                    .Where(k => k.Recognized.Any(u => u.ToPersonId == pUserId))
                    .OrderByDescending(k => k.Date)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .Include(l => l.Likes)
                    .Include(c => c.Comments)
                    .Include(u => u.UserFrom)
                    .Include(u => u.Recognized)
                    .Include(u => u.Recognition)
                    .ToListAsync();

        foreach (var kudo in kudos)
        {
            foreach (var like in kudo.Likes)
            {
                like.Person = _kudosDbContext.UserProfiles.First(u => u.UserProfileId == like.PersonId);
            }

            foreach (var receiver in kudo.Recognized)
            {
                receiver.Person = _kudosDbContext.UserProfiles.First(u => u.UserProfileId == receiver.ToPersonId);
            }
        }

        return kudos;
    }


    //this method is used to calculate the badge
    public IEnumerable<Domain.Models.Kudos> GetUserKudos(Guid pUserId)
    {

        return _kudosDbContext.Kudos.Where(k => k.Recognized.Any(u => u.ToPersonId == pUserId));

    }





}


