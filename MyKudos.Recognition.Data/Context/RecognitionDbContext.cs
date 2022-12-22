using Microsoft.EntityFrameworkCore;
using MyKudos.Recognition.Data.Data;
using MyKudos.Recognition.Domain.Models;


namespace MyKudos.Recognition.Data.Context;

public sealed class RecognitionDbContext : DbContext
{

    public RecognitionDbContext(DbContextOptions<RecognitionDbContext> options): base(options)
    {
    }

    public DbSet<MyKudos.Recognition.Domain.Models.Recognition> Recognitions { get; set; }

    
    
}
