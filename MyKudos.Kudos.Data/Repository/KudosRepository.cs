
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

	public Guid Add(KudosLog kudos)
	{
		_kudosDbContext.Kudos.Add(kudos);
		_kudosDbContext.SaveChanges();

		return kudos.Id;
	}

	public async Task<IEnumerable<KudosLog>> GetKudosAsync(int pageNumber = 1, int pageSize=5)
	{

		if(pageSize > _maxPageSize)
		{
			pageSize = _maxPageSize;
		}

		return await _kudosDbContext.Kudos.OrderByDescending(k => k.Date)
					.Skip(pageSize * (pageNumber - 1))
					.Take(pageSize)
					.ToListAsync();
	}

	public IQueryable<KudosLog> GetUserKudos(string pUserId)
	{
		
		return  _kudosDbContext.Kudos.Where(k => k.ToPersonId == pUserId).AsQueryable();
		
	}


	public bool Like(string kudosId, string personId)
	{

		var kudos = _kudosDbContext.Kudos.Where(k => k.Id == new Guid(kudosId)).First();

		if (kudos == null)
			return false;


		if (kudos.Likes == null)
		{
			kudos.Likes = new List<string>();
			kudos.Likes.Add(personId);
		}		
		else if (!kudos.Likes.Contains(personId	))
		{
			kudos.Likes.Add(personId);
		}

		return _kudosDbContext.SaveChanges() >0;

	}


    public bool UndoLike(string kudosId, string personId)
    {

        var kudos = _kudosDbContext.Kudos.Where(k => k.Id == new Guid(kudosId)).First();

        if ((kudos == null) || (kudos.Likes == null))
            return false;


        if (kudos.Likes.Contains(personId))
        {
            kudos.Likes.Remove(personId);
        }
        

        return _kudosDbContext.SaveChanges() > 0;

    }


    public bool SendComments(string kudosId, string commentId)
	{

		var kudos = _kudosDbContext.Kudos.Where(k => k.Id == new Guid(kudosId)).First();

		if (kudos == null)
			return false;

		if (kudos.Comments == null)
			kudos.Comments = new List<string>();

		kudos.Comments.Add(commentId);

        return _kudosDbContext.SaveChanges() >0;

	}

    public bool DeleteComments(string kudosId, Guid commentId)
    {
        var kudos = _kudosDbContext.Kudos.Where(k => k.Id == new Guid(kudosId)).First();

        if (kudos == null)
            return false;


        if ( (kudos.Comments != null) && (kudos.Comments.Contains(commentId.ToString()) ) )
		{
			kudos.Comments.Remove(commentId.ToString());
			return _kudosDbContext.SaveChanges() > 0;
		}

		return false;
    }
}
