using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceTypeController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceTypeDto>>> GetAllResourceTypes()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<ResourceTypeDto>> CreateResourceType(string code)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<ResourceTypeDto>> UpdateResourceType(int id, string code)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteResourceType(int id)
        {
            throw new NotImplementedException();
        }
    }
}
