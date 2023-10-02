using Microsoft.EntityFrameworkCore;
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


    public IEnumerable<RecognitionGroup> GetRecognitionsGroup()
    {
        return _context.RecognitionsGroup;
    }

    public bool AddNewRecognitionGroup(RecognitionGroup group)
    {
        _context.RecognitionsGroup.Add(group);

        return _context.SaveChanges() > 0;
    }

    public bool DeleteRecognitionGroup(int recognitionGroupId)
    {
        var group = _context.RecognitionsGroup.Find(recognitionGroupId);

        if (group == null)
        {
            return false;
        }

        _context.RecognitionsGroup.Remove(group);

        return  _context.SaveChanges() > 0;
    }

    public bool UpdateRecognitionGroup(RecognitionGroup group)
    {
        _context.Entry(group).State = EntityState.Modified;

        return _context.SaveChanges() > 0;
    }
}
