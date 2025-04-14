using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Redis;

namespace ReservationManager.API.Controllers;

public class TestRedis : Controller
{
    [HttpGet("test-redis")]
    public async Task<IActionResult> TestRedisCall([FromServices] IRedisService redis)
    {
        await redis.SetAsync("ping", "pong");
        var val = await redis.GetAsync("ping");
        return Ok(val);
    }
}