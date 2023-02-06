
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

    public bool SendLike(string kudosId, string personId)
    {
		var kudos = _context.Kudos.Where(k => k.Id == new Guid(kudosId)).First();

		if (kudos != null)
		{
			if (kudos.Likes == null)
			{
				kudos.Likes = new List<string>();
                kudos.Likes.Add(personId);
            }else if (kudos.Likes.Contains(personId))
			{
				kudos.Likes.Remove(personId);
			}
			else
			{
				kudos.Likes.Add(personId);
			}

			_context.SaveChanges();
		}
		return true;

    }
}
