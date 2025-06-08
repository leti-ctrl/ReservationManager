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

    #region Reservation
    public static IReservationService GetReservationService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationService>();
    }

    public static IReservationRepository GetReservationRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationRepository>();
    }
    #endregion

    #region ReservationType
    public static IReservationTypeService GetReservationTypeService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationTypeService>();
    }
    
    public static IReservationTypeRepository GetReservationTypeRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReservationTypeRepository>();
    }
    #endregion
    
    #region Resource
    public static IResourceService GetResourceService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IResourceService>();
    }
    
    public static IResourceRepository GetResourceRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IResourceRepository>();
    }
    #endregion
    
    #region ResourceType
    public static IResourceTypeService GetResourceTypeService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IResourceTypeService>();
    }
    
    public static IResourceTypeRepository GetResourceTypeRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IResourceTypeRepository>();
    }
    #endregion

    #region User
    public static IUserService GetUserService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUserService>();
    }

    public static IUserRepository GetUserRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUserRepository>();
    }
    #endregion

    #region Role
    public static IRoleService GetRoleService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRoleService>();
    }

    public static IRoleRepository GetRoleRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRoleRepository>();
    }
    #endregion

    #region ClosingCalendar
    public static IClosingCalendarService GetClosingCalendarService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IClosingCalendarService>();
    }

    public static IClosingCalendarFilterService GetClosingCalendarFilterService()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IClosingCalendarFilterService>();
    }

    public static IClosingCalendarRepository GetClosingCalendarRepository()
    {
        return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IClosingCalendarRepository>();
    }
    #endregion
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