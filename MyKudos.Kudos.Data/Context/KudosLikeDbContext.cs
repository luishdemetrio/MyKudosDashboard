using Microsoft.EntityFrameworkCore;


namespace MyKudos.Kudos.Data.Context;

public class KudosLikeDbContext : DbContext
{

    public KudosLikeDbContext(DbContextOptions<KudosLikeDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }



    public DbSet<Domain.Models.KudosLike> KudosLike { get; set; }


}