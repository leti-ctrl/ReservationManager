namespace ReservationManager.Cache.Helper;

public static class BuildKeyHelper
{
    public static string BuildKeyByTypeAndId(Type type, int id)
    {
        return type.Name + "_" + id;
    }

    public static string BuildKeyByTypeAndCode(Type type, string code)
    {
        return type.Name + "_" + code;
    }
    
    public static string BuildKeyByTypeIdAndValue(Type type, int id, Type valueType)
    {
        return type.Name + "_" + id + "_" + valueType.Name;
    }
}