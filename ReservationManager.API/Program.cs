using Hellang.Middleware.ProblemDetails;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ReservationManager.API;
using ReservationManager.API.Extensions;
using ReservationManager.Persistence;
using ConfigurationExtensions = ReservationManager.API.Extensions.ConfigurationExtensions;

TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddProblemDetails(
    options => ConfigurationExtensions.ConfigureProblemDetails(options, builder.Environment)
);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<EmailFilter>();
});
builder.Services.AddDbContext<ReservationManagerDbContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("ReservationManagerDb")));

builder.Services.ConfigureRepositories()
                .ConfigureServices()
                .ConfigureValidators()
                .ConfigureMappers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseProblemDetails();
await app.UseMigration<ReservationManagerDbContext>();
await app.UseSeed();


app.UseAuthorization();

app.MapControllers();

app.Run();
