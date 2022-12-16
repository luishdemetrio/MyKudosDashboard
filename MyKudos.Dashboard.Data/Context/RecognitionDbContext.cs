using Microsoft.EntityFrameworkCore;
using MyKudos.Dashboard.Data.Data;
using MyKudos.Dashboard.Domain.Models;


namespace MyKudos.Dashboard.Data.Context;

public sealed class RecognitionDbContext : DbContext
{

    public RecognitionDbContext(DbContextOptions<RecognitionDbContext> options): base(options)
    {
    }

    public DbSet<Recognition> Recognitions { get; set; }

    
    
}
