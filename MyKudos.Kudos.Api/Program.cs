using Microsoft.EntityFrameworkCore;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Communication.Helper.Services;
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

//builder.Services.AddGrpc(c => c.EnableDetailedErrors = true);

builder.Services.AddTransient<IKudosService, KudosService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<IRecognitionService, RecognitionService>();
builder.Services.AddTransient<IRecognitionGroupService, RecognitionGroupService>();

builder.Services.AddTransient<IUserPointsService, UserPointsService>();
builder.Services.AddTransient<IUserProfileService, UserProfileService>();

builder.Services.AddTransient<IKudosRepository, KudosRepository>();
builder.Services.AddTransient<IKudosLikeRepository, KudosLikeRepository>();
builder.Services.AddTransient<ICommentsRepository, CommentsRepository>();
builder.Services.AddTransient<IRecognitionRepository, RecognitionRepository>();
builder.Services.AddTransient<IRecognitionGroupRepository, RecognitionGroupRepository>();
builder.Services.AddTransient<IUserPointsRepository, UserPointsRepository>();
builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();

var config = builder.Configuration;

builder.Services.AddScoped<IRestClientHelper>(t =>
                new RestClientHelper(
                   new RestServiceToken(
                    clientId: config["ClientId"],
                    clientSecret: config["ClientSecret"],
                    tenantId: config["TenantId"],
                    exposedAPI: config["ExposedApi"]
                )
                ));


builder.Services.AddDbContext<KudosDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));

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

