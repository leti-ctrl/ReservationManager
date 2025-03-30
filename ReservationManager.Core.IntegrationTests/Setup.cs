using Microsoft.Extensions.DependencyInjection;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using Testcontainers.PostgreSql;

namespace ReservationManager.Core.IntegrationTests;

[SetUpFixture]
public partial class Setup
{
    private static IServiceScopeFactory _scopeFactory;
    
    [OneTimeSetUp]
    public async Task SetupAsync()
    {
        var db = new PostgresSqlServerTestcontainer();
        await db.InitializeAsync();
        var factory = new CustomWebApplicationFactory(db.ConnectionString);
        _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    }


    public static IReservationTypeService GetReservationTypeService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationTypeService>();
    }

    public static IReservationTypeRepository GetReservationTypeRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationTypeRepository>();
    }

    public static IReservationRepository GetReservationRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationRepository>();
    }

    public static IReservationService GetReservationService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationService>();
    }

    public static IResourceRepository GetResourceRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IResourceRepository>();
    }

    public static IResourceTypeRepository GetResourceTypeRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IResourceTypeRepository>();
    }

    public static IUserRepository GetUserRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUserRepository>();
    }
}

internal abstract class BaseSqlTestContainer
{
    public PostgreSqlContainer PostgresContainer { get; init; } = null!;

    /// <summary>
    /// Please use this ConnectionString instead of the ConnectionString from the Container itself
    /// </summary>
    internal string ConnectionString { get; set; } = null!;

    internal const string DbPassword = "localpassword@!123";

    //Arbitrary port since 1433 could be mapped to a local SQL Server so it can't bind the Docker Port
    internal const int Port = 1436;

    internal const string DbHostname = "sqlIntegration";

    public async Task InitializeAsync()
    {
        await PostgresContainer.StartAsync()
            .ConfigureAwait(false);

        ConnectionString = PostgresContainer.GetConnectionString();
    }

    public async Task DisposeAsync()
    {
        await PostgresContainer.StopAsync().ConfigureAwait(false);
        await PostgresContainer.DisposeAsync().ConfigureAwait(false);
    }
}

internal sealed class PostgresSqlServerTestcontainer : BaseSqlTestContainer
{
    internal PostgresSqlServerTestcontainer()
    {
        PostgresContainer = new PostgreSqlBuilder()
            .WithHostname(DbHostname)
            .WithPassword(DbPassword)
            .WithPortBinding(Port)
            .WithImage("postgres")
            .Build();

        ConnectionString = "";
    }
}