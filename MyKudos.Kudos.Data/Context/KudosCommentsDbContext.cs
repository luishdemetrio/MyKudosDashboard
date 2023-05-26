using Microsoft.EntityFrameworkCore;


namespace MyKudos.Kudos.Data.Context;

public class KudosCommentsDbContext : DbContext
{

    public KudosCommentsDbContext(DbContextOptions<KudosCommentsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }



    public DbSet<Domain.Models.Comments> Comments { get; set; }

}