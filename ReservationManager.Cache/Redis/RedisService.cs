﻿using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NRedisStack.DataTypes;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace ReservationManager.Cache.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ReservationManagerCache");
        var redis = ConnectionMultiplexer.Connect(connectionString!);
        _db = redis.GetDatabase();
    }

    public async Task SetAsync(string key, string value)
    {
        await _db.StringSetAsync(key, value, TimeSpan.FromMinutes(2));
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }
}