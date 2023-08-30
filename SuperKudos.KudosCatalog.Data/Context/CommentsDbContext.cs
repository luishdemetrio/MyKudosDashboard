using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Data.Context;

public class CommentsDbContext : DbContext

{


    public CommentsDbContext(DbContextOptions<CommentsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Comments> Comments { get; set; }

}
