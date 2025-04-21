namespace ReservationManager.Cache.Helper;

public static class BuildKeyHelper
{
    public static string BuildKey(Type type, int id)
    {
        return type.Name + "_" + id;
    }
}