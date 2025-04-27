using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Controllers.Base;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Dtos;

namespace ReservationManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LazyVsEagerController : SessionController
{
    private readonly ILazyVsEagerRepository _lazyVsEagerRepository;

    public LazyVsEagerController(ILazyVsEagerRepository lazyVsEagerRepository)
    {
        _lazyVsEagerRepository = lazyVsEagerRepository;
    }


    [HttpGet("getEager")]
    public async Task<ActionResult<IEnumerable<ResourceRepoDto>>> GetWithEagerLoading()
    {
        return Ok(await _lazyVsEagerRepository.EagerGetAllResourcesAsDtoAsync());
    }
    
    [HttpGet("getLazy")]
    public async Task<ActionResult<IEnumerable<ResourceRepoDto>>> GetWithLazyLoading()
    {
        return Ok(await _lazyVsEagerRepository.LazyGetAllResourcesAsDtoAsync());
    }

}