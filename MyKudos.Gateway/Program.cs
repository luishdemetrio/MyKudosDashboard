using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Communication.Helper.Services;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Queues;
using MyKudos.Gateway.Services;
using MyKudos.Gateway.Services.Rest;
using MyKudos.MessageSender.Interfaces;
using MyKudos.Queue.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGraphService, GraphServiceRest>();
builder.Services.AddScoped<IRecognitionService, RecognitionServiceRest>();
builder.Services.AddScoped<IKudosService, KudosServiceRest>();

builder.Services.AddScoped<IGamificationService, GamificationService>();
builder.Services.AddScoped<ICommentsService, CommentsServiceRest>();

builder.Services.AddScoped<IUserPointsService, UserPointsService>();

builder.Services.AddSingleton<ICommentsMessageSender, CommentsMessageSender>();

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

builder.Services.AddScoped<IKudosMessageSender, KudosMessageSender>();

builder.Services.AddSwaggerGen(c =>
{
   
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});


builder.Services.AddSingleton<IMessageSender>(t =>
                new ServiceBusMessageSender(
                    serviceBusConnectionString: config["KudosServiceBus_ConnectionString"]
                ));

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

app.UseCors();

app.Run();
