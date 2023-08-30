
using SuperKudos.KudosCatalog.Data.Context;
using SuperKudos.KudosCatalog.Data.Data;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.Data.Repository;

public sealed class RecognitionRepository : IRecognitionRepository
{

    private KudosDbContext _context;

    public RecognitionRepository(KudosDbContext context)
    {
        _context = context;
    }

    public IEnumerable<SuperKudos.KudosCatalog.Domain.Models.Recognition> GetRecognitions()
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
}
