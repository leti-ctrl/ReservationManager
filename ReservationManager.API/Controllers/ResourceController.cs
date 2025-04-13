using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Authorization;
using ReservationManager.API.Controllers.Base;
using ReservationManager.API.Request;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : SessionController
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.GeneralServices })]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetAllResources()
        {
            var resources = await _resourceService.GetAllResources();
            if(!resources.Any())
                return NoContent();
            
            return Ok(resources);
        }

        [HttpPost("filtered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.Employee })]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetFilteredResource([FromBody] ResourceFilterRequest resourceFilter)
        {
            var resources = await _resourceService.GetFilteredResources(resourceFilter.Adapt<ResourceFilterDto>());
            
            if(resources.Any())
                return Ok(resources);
            return NoContent();
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.GeneralServices })]
        public async Task<ActionResult<ResourceDto>> CreateResource(UpsertResourceDto resource)
        {
            var created = await _resourceService.CreateResource(resource);

            return Ok(created);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.GeneralServices })]
        public async Task<ActionResult<ResourceDto>> UpdateResource(int id, UpsertResourceDto resource)
        {
            var updated = await _resourceService.UpdateResource(id, resource);
            if(updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.GeneralServices })]
        public async Task<ActionResult> DeleteResource(int id)
        {
            await _resourceService.DeleteResource(id);
            return Accepted();
        }
    }
}
