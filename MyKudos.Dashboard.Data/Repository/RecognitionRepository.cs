using Microsoft.EntityFrameworkCore;
using MyKudos.Dashboard.Data.Context;
using MyKudos.Dashboard.Data.Data;
using MyKudos.Dashboard.Domain.Interfaces;
using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.Data.Repository;

public sealed class RecognitionRepository : IRecognitionRepository
{

    private RecognitionDbContext _context;

    public RecognitionRepository(RecognitionDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Recognition> GetRecognitions()
    {

        return _context.Recognitions;

    }

   

}
