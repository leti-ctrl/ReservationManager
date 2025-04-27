namespace ReservationManager.BenchmarkEagerVsLazy.QueryCount;

public static class QueryCounter
{
    public static readonly string EagerFile = @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\querycount_eager.txt";
    public static readonly string LazyFile  = @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\querycount_lazy.txt";

    private static int _eagerTotalCount;
    private static int _lazyTotalCount;

    private static int _eagerSnapshot;
    private static int _lazySnapshot;

    private static bool _isEagerContext = false;

    public static void StartEager() 
    {
        _isEagerContext = true;
        _eagerSnapshot = _eagerTotalCount; // <-- Salvi a che punto eri
    }

    public static void StartLazy() 
    {
        _isEagerContext = false;
        _lazySnapshot = _lazyTotalCount; // <-- Salvi a che punto eri
    }

    public static void Increment()
    {
        if (_isEagerContext)
            _eagerTotalCount++;
        else
            _lazyTotalCount++;
    }
    
    public static void DumpToFile(string path)
    {
        int executedQueries;
        if (_isEagerContext)
            executedQueries = _eagerTotalCount - _eagerSnapshot; // <-- Solo la differenza rispetto allo start
        else
            executedQueries = _lazyTotalCount - _lazySnapshot;    // <-- Solo la differenza rispetto allo start

        File.AppendAllText(path, executedQueries + Environment.NewLine);
    }
}