
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

    public bool Add(KudosLog kudos)
    {
		_context.Kudos.Add(kudos);
		_context.SaveChanges();
		return true;
    }

    public IEnumerable<KudosLog> GetKudos()
	{
		return _context.Kudos;
	}
}
