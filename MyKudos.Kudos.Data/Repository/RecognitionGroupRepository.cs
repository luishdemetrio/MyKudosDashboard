using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Data.Repository;

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
