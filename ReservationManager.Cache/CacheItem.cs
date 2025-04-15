using Newtonsoft.Json.Linq;
using NRedisStack.DataTypes;

namespace ReservationManager.Cache;

public class CacheItem
{
    public required TimeStamp Timestamp { get; set; }
    public required string JsonObjectValue { get; set; }
}