using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Domain.Models; 


namespace MyKudos.Kudos.Data.Context;

public class KudosDbContext : DbContext
{

    public KudosDbContext(DbContextOptions<KudosDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //used by the top contributors procedure
        modelBuilder.Entity<UserPoint>().HasNoKey().ToView(null); ;

        modelBuilder.Entity<Comments>()
                  .HasOne<UserProfile>(c => c.UserFrom)
                  .WithMany()
                  .HasForeignKey(c => c.FromPersonId)
                  .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<KudosLike>()
            .HasOne<UserProfile>(k => k.Person)
            .WithMany()
            .HasForeignKey(k => k.PersonId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CommentsLikes>()
            .HasOne<UserProfile>(k => k.Person)
            .WithMany()
            .HasForeignKey(k => k.FromPersonId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<KudosReceiver>()
            .HasOne<UserProfile>(k => k.Person)
            .WithMany()
            .HasForeignKey(k => k.ToPersonId)
            .OnDelete(DeleteBehavior.NoAction);


    }



    public DbSet<Domain.Models.Kudos> Kudos { get; set; }

    public DbSet<Comments> Comments { get; set; }

    public DbSet<Recognition> Recognitions { get; set; }

    public DbSet<Domain.Models.KudosLike> KudosLike { get; set; }

    public DbSet<CommentsLikes> CommentsLikes { get; set; }

    public DbSet<BadgeRules> BadgeRules{ get; set; }

    public DbSet<Points> Points { get; set; }

    public DbSet<RecognitionGroup> RecognitionsGroup { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<AdminUser> AdminUsers { get; set; }

}
