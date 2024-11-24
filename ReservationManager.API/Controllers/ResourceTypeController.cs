using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Request;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceTypeController : ControllerBase
    {
        private readonly IResourceTypeService _resourceTypeService;

        public ResourceTypeController(IResourceTypeService resourceService)
        {
            _resourceTypeService = resourceService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResourceTypeDto>>> GetAllResourceTypes()
        {
            var resourceTypes = await _resourceTypeService.GetAllResourceTypes();

            if (!resourceTypes.Any())
                return NoContent();
            return Ok(resourceTypes);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResourceTypeDto>> CreateResourceType(ResoruceTypeUpsertRequest request)
        {
            var resourceType = await _resourceTypeService.CreateResourceType(request.Adapt<UpsertResourceTypeDto>());
            return Ok(resourceType);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResourceTypeDto>> UpdateResourceType(int id, ResoruceTypeUpsertRequest request)
        {
            var updated = await _resourceTypeService.UpdateResourceType(id, request.Adapt<UpsertResourceTypeDto>());
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteResourceType(int id)
        {
            await _resourceTypeService.DeleteResourceType(id);
            return Accepted();
        }
    }
}
