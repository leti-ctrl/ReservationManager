namespace ReservationManager.BenchmarkEagerVsLazy.QueryCount;

public static class QueryCounter
{
    public static readonly string AllResourceEagerFilePath =
        @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\QueryCount\Files\all_resource_eager.txt";
    public static readonly string AllResourceLazyFilePath =
        @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\QueryCount\Files\all_resource_lazy.txt";
    private static int _allResourcesIsEagerContext = -1;
    private static int _allResourceEagerTotalCount;
    private static int _allResourceEagerSnapshot;
    private static int _allResourceLazyTotalCount;
    private static int _allResourceLazySnapshot;

    public static readonly string ResourceByIdEagerFilePath =
        @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\QueryCount\Files\resource_by_id_eager.txt";
    public static readonly string ResourceByIdLazyFilePath =
        @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\QueryCount\Files\resource_by_id_lazy.txt";
    private static int _resourceByIdIsEagerContext = -1;
    private static int _resourceByIdEagerTotalCount;
    private static int _resourceByIdEagerSnapshot;
    private static int _resourceByIdLazyTotalCount;
    private static int _resourceByIdLazySnapshot;
    
    public static readonly string TypeIdEagerFilePath =
        @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\QueryCount\Files\type_id_eager.txt";
    public static readonly string TypeIdLazyFilePath =
        @"C:\Users\angiu\Documents\ReservationManager\ReservationManager.BenchmarkEagerVsLazy\QueryCount\Files\type_id_lazy.txt";
    private static int _typeIdIsEagerContext = -1;
    private static int _typeIdEagerTotalCount;
    private static int _typeIdEagerSnapshot;
    private static int _typeIdLazyTotalCount;
    private static int _typeIdLazySnapshot;

    public static void StartEagerAllResources() 
    {
        _allResourcesIsEagerContext = 0;
        _allResourceEagerSnapshot = _allResourceEagerTotalCount; 
    }

    public static void StartLazyAllResources() 
    {
        _allResourcesIsEagerContext = 1;
        _allResourceLazySnapshot = _allResourceLazyTotalCount;
    }
    
    public static void StartEagerResourceById() 
    {
        _resourceByIdIsEagerContext = 0;
        _resourceByIdEagerSnapshot = _resourceByIdEagerTotalCount; 
    }

    public static void StartLazyResourceById() 
    {
        _resourceByIdIsEagerContext = 1;
        _resourceByIdLazySnapshot = _resourceByIdLazyTotalCount;
    }
    
    public static void StartEagerTypeId() 
    {
        _typeIdIsEagerContext = 0;
        _typeIdEagerSnapshot = _typeIdEagerTotalCount; 
    }

    public static void StartLazyTypeyId() 
    {
        _typeIdIsEagerContext = 1;
        _typeIdLazySnapshot = _typeIdLazyTotalCount;
    }

    public static void Increment()
    {
        if (_allResourcesIsEagerContext == 0)
            _allResourceEagerTotalCount++;
        if(_allResourcesIsEagerContext == 1)
            _allResourceLazyTotalCount++;
        
        if (_resourceByIdIsEagerContext == 0)
            _resourceByIdEagerTotalCount++;
        if(_resourceByIdIsEagerContext == 1)
            _resourceByIdLazyTotalCount++;
        
        if (_typeIdIsEagerContext == 0)
            _typeIdEagerTotalCount++;
        if(_typeIdIsEagerContext == 1)
            _typeIdLazyTotalCount++;
    }
    
    public static void DumpToFileAllResources(string path)
    {
        int executedQueries = -1;
        
        if (_allResourcesIsEagerContext == 0)
            executedQueries = _allResourceEagerTotalCount - _allResourceEagerSnapshot; 
        if (_allResourcesIsEagerContext == 1)
            executedQueries = _allResourceLazyTotalCount - _allResourceLazySnapshot;
        
        if (_resourceByIdIsEagerContext == 0)
            executedQueries = _resourceByIdEagerTotalCount - _resourceByIdEagerSnapshot;
        if(_resourceByIdIsEagerContext == 1)
            executedQueries = _resourceByIdLazyTotalCount - _resourceByIdLazySnapshot;
        
        if (_typeIdIsEagerContext == 0)
            executedQueries = _typeIdEagerTotalCount - _typeIdEagerSnapshot;
        if(_typeIdIsEagerContext == 1)
            executedQueries = _typeIdLazyTotalCount - _typeIdLazySnapshot;

        File.AppendAllText(path, executedQueries + Environment.NewLine);
    }
}