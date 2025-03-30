using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationManager.Persistence;

namespace ReservationManager.Core.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public string ContainerConnectionString { get; set; }
    public CustomWebApplicationFactory(string containerConnectionString)
    {
        ContainerConnectionString = containerConnectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        builder.ConfigureServices((_, services) =>
        {
            services.Remove<DbContextOptions<ReservationManagerDbContext>>()
                .AddDbContext<ReservationManagerDbContext>((_, options) =>
                    options.UseNpgsql(ContainerConnectionString,
                        optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ReservationManagerDbContext).Assembly.FullName)));

            services.EnsureDbCreated<ReservationManagerDbContext>();
        });
    }
}