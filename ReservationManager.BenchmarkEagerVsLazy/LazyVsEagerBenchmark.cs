using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using ReservationManager.BenchmarkEagerVsLazy.QueryCount;
using ReservationManager.Persistence;
using ReservationManager.Persistence.Repositories;

namespace ReservationManager.BenchmarkEagerVsLazy;

public class LazyVsEagerBenchmark
{
    private const string EagerFile = "./querycount_eager.txt";
    private const string LazyFile  = "./querycount_lazy.txt";
    private readonly LazyVsEagerRepository _repository;
    private readonly ReservationManagerDbContext _context;

    public LazyVsEagerBenchmark()
    {
        _context = new ReservationManagerDbContext(
            new DbContextOptionsBuilder<ReservationManagerDbContext>()
                .UseNpgsql("Host=localhost;Username=postgres;Password=RM123!;Database=ReservationManager;Port=5432")
                .UseLazyLoadingProxies()
                .AddInterceptors(new QueryCountingInterceptor()) 
                .Options);
        _repository = new LazyVsEagerRepository(_context);
    }

    [Benchmark(Baseline = true)]
    public int EagerLoading()
    {
        QueryCounter.StartEager();

        _repository.EagerGetAllResourcesAsDtoAsync().GetAwaiter().GetResult();;
        
        QueryCounter.DumpToFile(EagerFile);
        return QueryCounter.EagerCount;
    }

    [Benchmark]
    public int LazyLoading()
    {
        QueryCounter.StartLazy();
        
        _repository.LazyGetAllResourcesAsDtoAsync().GetAwaiter().GetResult();;
        
        QueryCounter.DumpToFile(LazyFile);
        return QueryCounter.LazyCount;
    }

}

