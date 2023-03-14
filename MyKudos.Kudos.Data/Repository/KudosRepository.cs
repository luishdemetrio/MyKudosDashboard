
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Repository;

public class KudosRepository: IKudosRepository
{

    private KudosDbContext _context;

	public KudosRepository(KudosDbContext context)
	{
		_context = context;
	}

    public Guid Add(KudosLog kudos)
    {
		_context.Kudos.Add(kudos);
		_context.SaveChanges();

		return kudos.Id;
    }

    public IEnumerable<KudosLog> GetKudos()
	{
		return _context.Kudos;
	}

	//return -1 means that the user unliked
	//return 1 means that the user liked
    public int SendLike(string kudosId, string personId)
    {

		int sign = 0;

		var kudos = _context.Kudos.Where(k => k.Id == new Guid(kudosId)).First();

		if (kudos != null)
		{
			if (kudos.Likes == null)
			{
				kudos.Likes = new List<string>();
                kudos.Likes.Add(personId);
				sign = 1;
            }else if (kudos.Likes.Contains(personId))
			{
				kudos.Likes.Remove(personId);
				sign = -1;
			}
			else
			{
				kudos.Likes.Add(personId);
				sign = 1;
			}

			_context.SaveChanges();
		}

		return sign;

    }
}
