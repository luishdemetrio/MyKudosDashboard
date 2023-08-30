using SuperKudos.KudosCatalog.Data.Context;
using SuperKudos.KudosCatalog.Domain.Interfaces;


namespace SuperKudos.KudosCatalog.Data.Repository;

public class KudosLikeRepository : IKudosLikeRepository
{

    private KudosDbContext _context;

    public KudosLikeRepository(KudosDbContext kudosLikeDbContext)
    {
        _context = kudosLikeDbContext;
    }

    public bool Like(int kudosId, Guid personId)
    {

        bool result = true;

        var kudos = _context.KudosLike.Where(k => k.KudosId == kudosId && k.PersonId == personId);


        if (kudos.Count() == 0 )
        {
            _context.KudosLike.Add(new Domain.Models.KudosLike  { KudosId = kudosId, PersonId = personId });
            
            result = _context.SaveChanges() > 0; 
        }

        return result;

    }


    public bool UndoLike(int kudosId, Guid personId)
    {
        bool result = true;

        var kudos = _context.KudosLike.Where(k => k.KudosId == kudosId && k.PersonId == personId).FirstOrDefault();

        
        if (kudos != null)
        {
            _context.KudosLike.Remove(kudos);

            result = _context.SaveChanges() > 0;
        }

        return result;

    }
}
