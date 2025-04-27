using System.Text.Json;

namespace ReservationManager.BenchmarkEagerVsLazy.QueryCount;

public static class QueryCounter
{
    public static int EagerCount { get; private set; }
    public static int LazyCount { get; private set; }

    private static bool _isEagerContext = false;

    public static void StartEager() 
    {
        _isEagerContext = true;
    }

    public static void StartLazy() 
    {
        _isEagerContext = false;
    }

    public static void Increment()
    {
        if (_isEagerContext)
            EagerCount++;
        else
            LazyCount++;
    }
    
    public static void DumpToFile(string path)
    {
        File.AppendAllText(
            path,
            (_isEagerContext ? EagerCount : LazyCount)
            + System.Environment.NewLine);
    }
}