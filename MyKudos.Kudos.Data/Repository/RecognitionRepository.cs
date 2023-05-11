
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Data.Data;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.Data.Repository;

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
