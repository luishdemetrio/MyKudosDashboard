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

   

}
