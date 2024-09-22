using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetResource(FilterDto filter)
        {
            throw new NotImplementedException();
        }

        [HttpGet("availability")]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetAvailabilityResource(FilterDto filter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<ResourceDto>> CreateResource(UpsertResourceDto resource)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<ResourceDto>> UpdateResource(int id, UpsertResourceDto resource)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteResource(int id)
        {
            throw new NotImplementedException();
        }
    }
}
