
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

    public Domain.Models.Kudos? GetKudos(int kudosId)
    {

        IQueryable<Domain.Models.Kudos> kudosQuery = 
            _kudosDbContext.Kudos
               .Include(c => c.Comments)
               .Include(u => u.UserFrom)
               .Include(u => u.Recognition)
               .Include(r => r.Recognized).ThenInclude(p => p.Person)
               .Include(r => r.Likes).ThenInclude(p => p.Person);

        kudosQuery = kudosQuery.Where(k => k.KudosId == kudosId);

        return  kudosQuery.First();
    }

    public async Task<IEnumerable<Domain.Models.Kudos>> GetAllKudosAsync(Guid pUserId, int pageNumber = 1, 
                                                                        int pageSize = 5, 
                                                                        bool fromMe = true,
                                                                        Guid? managerId = null,
                                                                        int? year = null)
    {
        if (pageSize > _maxPageSize)
        {
            pageSize = _maxPageSize;
        }

        IQueryable<Domain.Models.Kudos> kudosQuery = _kudosDbContext.Kudos
            .Include(c => c.Comments)
            .Include(u => u.UserFrom)
            .Include(u => u.Recognition);


        if (!year.HasValue)
            year = DateTime.Today.Year;

        kudosQuery = kudosQuery.Where(k => k.SentOnYear == year);

        kudosQuery = kudosQuery.Include(r => r.Recognized).ThenInclude(p => p.Person);
        kudosQuery = kudosQuery.Include(r => r.Likes).ThenInclude(p => p.Person);

        if (managerId != null)
        {

            kudosQuery = KudosFilterRules.FilterKudosByManagerOrProfile(kudosQuery, managerId);

        }
        else if (pUserId != Guid.Empty)
        {
            kudosQuery = kudosQuery.Where(k => fromMe ? k.FromPersonId == pUserId : k.Recognized.Any(u => u.ToPersonId == pUserId));
        }

        
        if (pageSize == 0)
        {
            //It will return all records
            //It happens when user clicks on Export
            var kudos = await kudosQuery
                .OrderByDescending(k => k.Date)
                .ToListAsync();
            return kudos;
        }
        else
        {
            var kudos = await kudosQuery
                .OrderByDescending(k => k.Date)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return kudos;
        }

    }

    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize=5,
                                                                      Guid? managerId = null,
                                                                      int? year = null)
	{
        return await GetAllKudosAsync(Guid.Empty, pageNumber, pageSize, fromMe: false, managerId, year);
    }

    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, 
                                                                            int pageSize = 5,
                                                                            Guid? managerId = null,
                                                                            int? year = null)
    {
        return await GetAllKudosAsync(pUserId, pageNumber, pageSize, fromMe: true, managerId: managerId, year);
    }

    public async Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, 
                                                                          int pageSize = 5, 
                                                                          Guid? managerId = null,
                                                                          int? year = null)
    {
        return await GetAllKudosAsync(pUserId, pageNumber, pageSize, fromMe: false, managerId: managerId, year);
    }

    //this method is used to calculate the badge
    public  IEnumerable<Domain.Models.Kudos> GetUserKudos(Guid pUserId, int? year = null)
    {

        IQueryable<Domain.Models.Kudos> kudosQuery = _kudosDbContext.Kudos
           .Include(c => c.Comments)
           .Include(u => u.UserFrom)
           .Include(u => u.Recognition);

        if (!year.HasValue)
            year = DateTime.Today.Year;

        kudosQuery = kudosQuery.Where(k => k.SentOnYear == year);

        kudosQuery = kudosQuery.Include(r => r.Recognized).ThenInclude(p => p.Person);
        kudosQuery = kudosQuery.Include(r => r.Likes).ThenInclude(p => p.Person);

        kudosQuery = kudosQuery.Where(k => k.Recognized.Any(u => u.ToPersonId == pUserId));
        
        return kudosQuery.ToList();

    }

    public bool UpdateMessage(int kudosId, string? message)
    {
        bool result = false;

        var kudos = _kudosDbContext.Kudos.FirstOrDefault(k => k.KudosId == kudosId);

        if (kudos != null)
        {
            kudos.Message = message;
            
            result = _kudosDbContext.SaveChanges() > 0;
        }

        return result;
    }

    public bool Delete(int kudosId)
    {

        bool result = false;

        var kudos = _kudosDbContext.Kudos.FirstOrDefault(k=> k.KudosId == kudosId);

        if (kudos != null)
        {
            _kudosDbContext.Kudos.Remove(kudos);
            result = _kudosDbContext.SaveChanges() > 0 ;
        }

        return result;
    }
}


