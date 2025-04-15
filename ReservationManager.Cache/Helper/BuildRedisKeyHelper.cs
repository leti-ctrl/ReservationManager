namespace ReservationManager.Cache.Helper;

public static class BuildRedisKeyHelper
{
    public static string BuildKey(Type type, int id)
    {
        return type.Name + "_" + id;
    }
}