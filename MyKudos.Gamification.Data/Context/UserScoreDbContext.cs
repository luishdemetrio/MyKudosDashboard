using Microsoft.EntityFrameworkCore;
using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Data.Context;


public class UserScoreDbContext : DbContext
{

    public UserScoreDbContext(DbContextOptions<UserScoreDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<UserScore> UserScores { get; set; }

}