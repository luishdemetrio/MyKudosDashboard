using Microsoft.EntityFrameworkCore;
using MyKudos.Dashboard.Data.Data;
using MyKudos.Dashboard.Domain.Models;


namespace MyKudos.Dashboard.Data.Context;

public sealed class RecognitionDbContext : DbContext
{

    //public RecognitionDbContext()
    //{

    //}
    public RecognitionDbContext(DbContextOptions<RecognitionDbContext> options): base(options)
    {
    }

    public DbSet<Recognition> Recognitions { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //   => optionsBuilder.UseCosmos(
    //       "https://mykudos.documents.azure.com:443/",
    //       "pPT5EVtJyAh0Lk4N7ywHk2ZgPTSepeH6YvbUYw2R6msjLeCQLHMs1KfhOE5xPdoHUQVR3vMFiXvmACDbOWmCqA==",
    //       databaseName: "kudosdb");

   


    public static async Task CheckAndSeedDatabaseAsync(DbContextOptions<RecognitionDbContext> options)
    {
        using var context = new RecognitionDbContext(options);

        var _ = await context.Database.EnsureDeletedAsync();

        if (await context.Database.EnsureCreatedAsync())
        {
            context.Recognitions?.AddRange(Seed.Data);

            await context.SaveChangesAsync();
        }
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
        
    //    modelBuilder.Entity<Recognition>()
    //        .ToContainer("Recognitions")
    //        .HasPartitionKey(c => c.Id);
     
       
        
    //}
}
