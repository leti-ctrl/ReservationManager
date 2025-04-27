using System.Data.Common;
using  Microsoft.EntityFrameworkCore.Diagnostics;

namespace ReservationManager.BenchmarkEagerVsLazy.QueryCount;



public class QueryCountingInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        QueryCounter.Increment();
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        QueryCounter.Increment();
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}
