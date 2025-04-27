namespace ReservationManager.BenchmarkEagerVsLazy.QueryCount;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

public class QueryCountColumn : IColumn
{
    public string Id => nameof(QueryCountColumn);
    public string ColumnName => "QueryCount";

    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory => 0;
    public bool IsNumeric => true;
    public UnitType UnitType => UnitType.Size;
    public string Legend => "Number of SQL queries executed";

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        // Scegli il file in base al metodo
        var file = benchmarkCase.Descriptor.WorkloadMethod.Name == nameof(LazyVsEagerBenchmark.EagerLoading)
            ? "querycount_eager.txt"
            : "querycount_lazy.txt";

        if (!File.Exists(file))
            return "N/A";

        // Legge tutte le righe in un array di stringhe :contentReference[oaicite:4]{index=4}
        var lines = File.ReadAllLines(file);
        int countLines = lines.Length;
        if (countLines == 0)
            return "0";

        // Parse dell'ultima riga
        if (!int.TryParse(lines[countLines - 1], out var totalQueries))
            return "err";

        // Media = totale / numero di iterazioni
        double avg = (double)totalQueries / countLines;
        return avg.ToString("F2");
    }

    // Overload per SummaryStyle, se necessario
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        => GetValue(summary, benchmarkCase);

    public bool IsAvailable(Summary summary) => true;
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
}
