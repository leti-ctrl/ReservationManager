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
    internal string ConnectionString { get; set; } = null!;
    internal const string DbPassword = "localpassword@!123";
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
        var randomPort = GetFreePort();
        PostgresContainer = new PostgreSqlBuilder()
            .WithHostname(DbHostname)
            .WithPassword(DbPassword)
            .WithPortBinding(randomPort, 5432)
            .WithImage("postgres")
            .Build();

        ConnectionString = "";
    }
    
    private static int GetFreePort()
    {
        using var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}