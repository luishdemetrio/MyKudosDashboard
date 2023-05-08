using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Domain.Models; 


namespace MyKudos.Kudos.Data.Context;

public class KudosDbContext : DbContext
{

    public KudosDbContext(DbContextOptions<KudosDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<KudosLog> Kudos { get; set; }  


}
