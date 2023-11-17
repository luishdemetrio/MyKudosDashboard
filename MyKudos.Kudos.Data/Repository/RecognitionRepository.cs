
using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Data.Data;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Repository;

public sealed class RecognitionRepository : IRecognitionRepository
{

    private KudosDbContext _context;

    public RecognitionRepository(KudosDbContext context)
    {
        _context = context;
    }

    public bool DeleteRecognition(int recognitionId)
    {
        var recognition = _context.Recognitions.Find(recognitionId);

        if (recognition == null)
        {
            return false;
        }

        _context.Recognitions.Remove(recognition);

        return _context.SaveChanges() > 0;
    }

    public IEnumerable<Domain.Models.Recognition> GetRecognitions()
    {

        return _context.Recognitions;

    }

    public async Task SeedDatabaseAsync()
    {
       
        var _ = await _context.Database.EnsureDeletedAsync();

        if (await _context.Database.EnsureCreatedAsync())
        {
            _context.Recognitions?.AddRange(Seed.Data);

            await _context.SaveChangesAsync();
        }
    }

    public bool SetRecognition(Recognition recognition)
    {
        _context.Recognitions.Add(recognition);

        return _context.SaveChanges() > 0;
    }

    public bool UpdateRecognition(Recognition recognition)
    {
        _context.Entry(recognition).State = EntityState.Modified;

        return _context.SaveChanges() > 0;
    }
}
