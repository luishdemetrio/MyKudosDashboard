using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.App.Services;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Data.Repository;
using MyKudos.Kudos.Domain.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

//builder.Services.AddMediatR(typeof(Program));

builder.Services.AddGrpc(c => c.EnableDetailedErrors = true);

builder.Services.AddScoped<IKudosService, KudosService>();
builder.Services.AddScoped<IKudosRepository, KudosRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();

var config = builder.Configuration.GetSection("CosmosDb");

builder.Services.AddScoped<KudosDbContext>(_ =>
{
    var options = new DbContextOptionsBuilder<KudosDbContext>()
      .UseCosmos(
              config["AccountEndPoint"],
              config["AccountKey"],
              config["DatabaseName"])
      .Options;

    return new KudosDbContext(options);
});


builder.Services.AddScoped<CommentsDbContext>(_ =>
{
    var options = new DbContextOptionsBuilder<CommentsDbContext>()
      .UseCosmos(
              config["AccountEndPoint"],
              config["AccountKey"],
              config["CommentsDatabaseName"])
      .Options;

    using var commentsContext = new CommentsDbContext(options);
    commentsContext.Database.EnsureCreated();

    return new CommentsDbContext(options);
});


builder.Services.AddScoped<IRecognitionService, RecognitionService>();
builder.Services.AddScoped<IRecognitionRepository, RecognitionRepository>();

builder.Services.AddDbContext<RecognitionDbContext>(options =>
{
    options.UseCosmos(
              config["AccountEndPoint"],
              config["AccountKey"],
              config["RecognitionDatabaseName"]);
});



var app = builder.Build();







// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//ConfigureEventBus(app);

//app.MapGrpcService<KudosGrpc>();

app.Run();


//void ConfigureEventBus(IApplicationBuilder app)
//{
//    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

//    eventBus.Subscribe<SendKudosCreatedEvent, SendKudosEventHandler>();
//}

