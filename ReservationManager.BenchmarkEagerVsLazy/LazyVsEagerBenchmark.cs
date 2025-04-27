using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using ReservationManager.BenchmarkEagerVsLazy.QueryCount;
using ReservationManager.Persistence;
using ReservationManager.Persistence.Repositories;

namespace ReservationManager.BenchmarkEagerVsLazy;

public class LazyVsEagerBenchmark
{
    [Benchmark(Baseline = true)]
    public void EagerLoading()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .UseLazyLoadingProxies()
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartEager();

        repository.EagerGetAllResourcesAsDtoAsync().GetAwaiter().GetResult();
    
        QueryCounter.DumpToFile(QueryCounter.EagerFile);
    }

    [Benchmark]
    public void LazyLoading()
    {
        using var context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .UseLazyLoadingProxies()
                .AddInterceptors(new QueryCountingInterceptor())
                .Options);
        var repository = new LazyVsEagerRepository(context);

        QueryCounter.StartLazy();
    
        repository.LazyGetAllResourcesAsDtoAsync().GetAwaiter().GetResult();
    
        QueryCounter.DumpToFile(QueryCounter.LazyFile);
    }
}

