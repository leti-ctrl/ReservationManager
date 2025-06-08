using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using ReservationManager.BenchmarkEagerVsLazy.QueryCount;
using ReservationManager.Persistence;
using ReservationManager.Persistence.Repositories;

namespace ReservationManager.BenchmarkEagerVsLazy;

public class LazyVsEagerBenchmark
{
    [Benchmark(Baseline = true)]
    public void Eager_GetAllBusyResourcesFromToday()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartEagerAllResources();

        repository.EagerGetAllBusyResourcesFromTodayAsync().GetAwaiter().GetResult();
    
        QueryCounter.DumpToFileAllResources(QueryCounter.AllResourceEagerFilePath);
    }

    [Benchmark]
    public void Lazy_GetAllBusyResourcesFromToday()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .UseLazyLoadingProxies()
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartLazyAllResources();
    
        repository.LazyGetAllBusyResourcesFromTodayAsync().GetAwaiter().GetResult();
    
        QueryCounter.DumpToFileAllResources(QueryCounter.AllResourceLazyFilePath);
    }
    
    [Benchmark]
    public void Eager_GetBusyResource_ByResourceIdAndDay()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartEagerResourceById();

        repository.EagerGetBusyResourceByResourceIdAndDayAsync(42, new DateOnly(2025, 09, 18)).GetAwaiter().GetResult();
    
        QueryCounter.DumpToFileAllResources(QueryCounter.ResourceByIdEagerFilePath);
    }

    [Benchmark]
    public void Lazy_GetBusyResource_ByResourceIdAndDay()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .UseLazyLoadingProxies()
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartLazyResourceById();
    
        repository.LazyGetBusyResourceByResourceIdAndDayAsync(42, new DateOnly(2025, 09, 18)).GetAwaiter().GetResult();
    
        QueryCounter.DumpToFileAllResources(QueryCounter.ResourceByIdLazyFilePath);
    }
    
    [Benchmark]
    public void Eager_GetBusyResource_ByResourceTypeIdAndDay()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartEagerTypeId();

        repository.EagerGetBusyResourceByResourceTypeIdAndDayAsync(3, new DateOnly(2025, 08, 15)).GetAwaiter().GetResult();
    
        QueryCounter.DumpToFileAllResources(QueryCounter.TypeIdEagerFilePath);
    }

    [Benchmark]
    public void Lazy_GetBusyResource_ByResourceTypeIdAndDay()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .UseLazyLoadingProxies()
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartLazyTypeyId();
    
        repository.LazyGetBusyResourceByResourceTypeIdAndDayAsync(3, new DateOnly(2025, 08, 15)).GetAwaiter().GetResult();
    
        QueryCounter.DumpToFileAllResources(QueryCounter.TypeIdLazyFilePath);
    }
}

