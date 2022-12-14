using MyKudos.Dashboard.Data.Context;
using MyKudos.Dashboard.Domain.Interfaces;
using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.Data.Repository;

public sealed class RecognitionRepository : IRecognitionRepository
{

    private RecognitionDbContext _context;

    public RecognitionRepository(RecognitionDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Recognition> GetRecognitions()
    {

        return _context.Recognitions;

    }

    //private void CheckAndSeedDatabaseAsync()
    //{
        //var options = new DbContextOptionsBuilder<RecognitionDbContext>()
        //    .UseCosmos(
        //            "https://mykudos.documents.azure.com:443/",
        //            "pPT5EVtJyAh0Lk4N7ywHk2ZgPTSepeH6YvbUYw2R6msjLeCQLHMs1KfhOE5xPdoHUQVR3vMFiXvmACDbOWmCqA==",
        //            databaseName: "kudosdb")
        //    .Options;

        //await RecognitionDbContext.CheckAndSeedDatabaseAsync(options);
    //}
}
