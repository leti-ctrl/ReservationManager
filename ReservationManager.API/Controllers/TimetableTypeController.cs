using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableTypeController : ControllerBase
    {
        private readonly ITimetableTypeService _timetableTypeService;

        public TimetableTypeController(ITimetableTypeService timetableTypeService)
        {
            _timetableTypeService = timetableTypeService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResourceTypeDto>>> GetAllResourceTypes()
        {
            var timetableTypes = await _timetableTypeService.GetAllTypes();

            if (timetableTypes == null)
                return NoContent();
            return Ok(timetableTypes);
        }
    }
}
