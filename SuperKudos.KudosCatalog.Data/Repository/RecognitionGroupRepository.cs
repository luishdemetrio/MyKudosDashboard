using SuperKudos.KudosCatalog.Data.Context;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.KudosCatalog.Data.Repository;

public class RecognitionGroupRepository : IRecognitionGroupRepository
{

    private KudosDbContext _context;

    public RecognitionGroupRepository(KudosDbContext context)
    {
        _context = context;
    }


    public IEnumerable<RecognitionGroup> GetRecognitionGroups()
    {
        return _context.RecognitionsGroup;
    }

    public bool SetRecognitionGroups(RecognitionGroup group)
    {
        _context.RecognitionsGroup.Add(group);

        return _context.SaveChanges() > 0;
    }
}
