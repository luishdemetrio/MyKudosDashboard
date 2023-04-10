
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Repository;

public class KudosRepository : IKudosRepository
{

	private KudosDbContext _kudosDbContext;



	public KudosRepository(KudosDbContext kudosDbContext)
	{
		_kudosDbContext = kudosDbContext;
		
	}

	public Guid Add(KudosLog kudos)
	{
		_kudosDbContext.Kudos.Add(kudos);
		_kudosDbContext.SaveChanges();

		return kudos.Id;
	}

	public IEnumerable<KudosLog> GetKudos()
	{
		return _kudosDbContext.Kudos;
	}

	//return -1 means that the user unliked
	//return 1 means that the user liked
	public int SendLike(string kudosId, string personId)
	{

		int sign = 0;

		var kudos = _kudosDbContext.Kudos.Where(k => k.Id == new Guid(kudosId)).First();

		if (kudos != null)
		{
			if (kudos.Likes == null)
			{
				kudos.Likes = new List<string>();
				kudos.Likes.Add(personId);
				sign = 1;
			}
			else if (kudos.Likes.Contains(personId))
			{
				kudos.Likes.Remove(personId);
				sign = -1;
			}
			else
			{
				kudos.Likes.Add(personId);
				sign = 1;
			}

			_kudosDbContext.SaveChanges();
		}

		return sign;

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
