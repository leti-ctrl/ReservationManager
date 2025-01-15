using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Services;

namespace ReservationManager.API.Controllers;

public class DllLoaderController : ControllerBase
{
    private readonly IRepositoryProvider _iRepositoryProvider;

    public DllLoaderController(IRepositoryProvider iRepositoryProvider)
    {
        _iRepositoryProvider = iRepositoryProvider;
    }

    [HttpGet("/getAllSpike")]
    public IActionResult GetAllSpike(string repositoryType)
    {
        try
        {
            _iRepositoryProvider.SwitchRepository(repositoryType);
            var service = new MockReservationService(_iRepositoryProvider.CurrentRepository);
            return Ok(service.GetAllReservations());
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}