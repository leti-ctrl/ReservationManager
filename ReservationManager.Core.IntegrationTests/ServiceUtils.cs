using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ReservationManager.Core.IntegrationTests;

public static class ServiceUtils
{
    public static IServiceCollection Remove<TService>(this IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(TService));

        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }

        return services;
    }
    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<T>();


        context.Database.Migrate();
    }
}