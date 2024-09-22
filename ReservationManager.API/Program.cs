using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using ReservationManager.API.Extensions;
using ReservationManager.Persistence;
using ConfigurationExtensions = ReservationManager.API.Extensions.ConfigurationExtensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddProblemDetails(
    options => ConfigurationExtensions.ConfigureProblemDetails(options, builder.Environment)
);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ReservationManagerDbContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("ReservationManagerDb")));

builder.Services.ConfigureRepositories()
                .ConfigureServices();


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

app.Run();
