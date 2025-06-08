using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ReservationManager.BenchmarkEagerVsLazy.QueryCount;

namespace ReservationManager.BenchmarkEagerVsLazy;

public class Program
{
    public static void Main(string[] args)
    {

        var config = ManualConfig
            .Create(DefaultConfig.Instance)
            .WithArtifactsPath(@"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\BenchmarkDotNet.Artifacts\20250602_results")
            .WithOptions(ConfigOptions.DisableOptimizationsValidator) // utile per evitare warning in debug
            .AddColumn(new QueryCountColumn()); 
        
        // Esegui il benchmark della classe LazyVsEagerBenchmark
        var summary = BenchmarkRunner.Run<LazyVsEagerBenchmark>(config);
        
        // Opzionale: stampa un riassunto del risultato del benchmark
        Console.WriteLine(summary);
    }
}