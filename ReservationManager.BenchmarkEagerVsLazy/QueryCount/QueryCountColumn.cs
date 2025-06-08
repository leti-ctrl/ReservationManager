namespace ReservationManager.BenchmarkEagerVsLazy.QueryCount;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

public class QueryCountColumn : IColumn
{
    public string Id => nameof(QueryCountColumn);
    public string ColumnName => "TotalQueryCount";

    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory => 0;
    public bool IsNumeric => true;
    public UnitType UnitType => UnitType.Size;
    public string Legend => "Total number of queries executed";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var file = "N/A";
        switch (benchmarkCase.Descriptor.WorkloadMethod.Name)
        {
            case nameof(LazyVsEagerBenchmark.Eager_GetAllBusyResourcesFromToday):
                file = QueryCounter.AllResourceEagerFilePath;
                break;
            case nameof(LazyVsEagerBenchmark.Lazy_GetAllBusyResourcesFromToday):
                file = QueryCounter.AllResourceLazyFilePath;
                break;
            case nameof(LazyVsEagerBenchmark.Eager_GetBusyResource_ByResourceIdAndDay):
                file = QueryCounter.ResourceByIdEagerFilePath;
                break;
            case nameof(LazyVsEagerBenchmark.Lazy_GetBusyResource_ByResourceIdAndDay):
                file = QueryCounter.ResourceByIdLazyFilePath;
                break;
            case nameof(LazyVsEagerBenchmark.Eager_GetBusyResource_ByResourceTypeIdAndDay):
                file = QueryCounter.TypeIdEagerFilePath;
                break;
            case nameof(LazyVsEagerBenchmark.Lazy_GetBusyResource_ByResourceTypeIdAndDay):
                file = QueryCounter.TypeIdLazyFilePath;
                break;
            default:
                file = "N/A";
                break;
        }
        
        if (!File.Exists(file))
            return "N/A";

        var lines = File.ReadAllLines(file);
        if (lines.Length == 0)
            return "0";

        var numbers = lines
            .Select(line => int.TryParse(line, out var n) ? n : 0)
            .ToList();

        if (numbers.Count == 0)
            return "0";

        var average = numbers.Average(); // Calcola la media
        return average.ToString("F2");    // Formatta con 2 cifre decimali
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        => GetValue(summary, benchmarkCase);

    public bool IsAvailable(Summary summary) => true;
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
}
