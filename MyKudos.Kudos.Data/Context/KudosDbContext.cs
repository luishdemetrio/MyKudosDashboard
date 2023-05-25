using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Domain.Models; 


namespace MyKudos.Kudos.Data.Context;

public class KudosDbContext : DbContext
{

    public KudosDbContext(DbContextOptions<KudosDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    

    public DbSet<Domain.Models.Kudos> Kudos { get; set; }

    public DbSet<Comments> Comments { get; set; }

    public DbSet<Recognition> Recognitions { get; set; }

    public DbSet<Domain.Models.KudosLike> KudosLike { get; set; }

    public DbSet<CommentsLikes> CommentsLikes { get; set; }

    public DbSet<UserScore> UserScores { get; set; }


}
