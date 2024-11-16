using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetAllResources()
        {
            var resources = await _resourceService.GetAllResources();
            if(!resources.Any())
                return NoContent();
            
            return Ok(resources);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResourceDto>> GetResource(int id)
        {
            var resource = await _resourceService.GetResourceById(id);

            return Ok(resource);
        }

        [HttpPost("filtered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetFilteredResource([FromBody]ResourceFilterDto resourceFilter)
        {
            var resources = await _resourceService.GetFilteredResources(resourceFilter);
            
            if(resources.Any())
                return Ok(resources);
            return NoContent();
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResourceDto>> CreateResource(UpsertResourceDto resource)
        {
            var created = await _resourceService.CreateResource(resource);

            return Ok(created);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResourceDto>> UpdateResource(int id, UpsertResourceDto resource)
        {
            var updated = await _resourceService.UpdateResource(id, resource);

            return Ok(updated);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteResource(int id)
        {
            throw new NotImplementedException();
        }
    }
}
