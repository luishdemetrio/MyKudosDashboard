using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Context;

public sealed class RecognitionDbContext : DbContext
{

    public RecognitionDbContext(DbContextOptions<RecognitionDbContext> options): base(options)
    {
    }

    public DbSet<Recognition> Recognitions { get; set; }

    
    
}
