using Microsoft.EntityFrameworkCore;
using MyKudos.Recognition.Data.Context;
using MyKudos.Recognition.Data.Data;
using MyKudos.Recognition.Domain.Interfaces;

namespace MyKudos.Recognition.Data.Repository;

public sealed class RecognitionRepository : IRecognitionRepository
{

    private RecognitionDbContext _context;

    public RecognitionRepository(RecognitionDbContext context)
    {
        _context = context;
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
}
