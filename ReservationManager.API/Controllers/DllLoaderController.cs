using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Services;

namespace ReservationManager.API.Controllers;

public class DllLoaderController : ControllerBase
{
    private readonly IRepositorySwitcher _repositorySwitcher;

    public DllLoaderController(IRepositorySwitcher repositorySwitcher)
    {
        _repositorySwitcher = repositorySwitcher;
    }

    [HttpGet("/getAllSpike")]
    public IActionResult GetAllSpike()
    {
        var service = new MockReservationService(_repositorySwitcher.CurrentRepository);
        return Ok(service.GetAllReservations());
    }

    [HttpPost("/switch-repository")]
    public IActionResult SwitchRepository(string repositoryType)
    {
        try
        {
            _repositorySwitcher.SwitchRepository(repositoryType);
            return Ok($"Repository cambiato a {repositoryType}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}