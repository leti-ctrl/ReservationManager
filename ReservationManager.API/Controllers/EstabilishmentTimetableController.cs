using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Request;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstabilishmentTimetableController : ControllerBase
    {
        private readonly IEstabilishmentTimetableService _estabilishmentTimetableService;

        public EstabilishmentTimetableController(IEstabilishmentTimetableService estabilishmentTimetableService)
        {
            _estabilishmentTimetableService = estabilishmentTimetableService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EstabilishmentTimetableDto>>> GetAll()
        {
            var ret = await _estabilishmentTimetableService.GetAll();
            return Ok(ret);
        }

        [HttpGet("{typeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<IEnumerable<EstabilishmentTimetableDto>>> GetByType(int typeId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("dateRange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<IEnumerable<EstabilishmentTimetableDto>>> GetByDateRange(DateOnly start, DateOnly end)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstabilishmentTimetableDto>> Create(UpsertEstabilishmentTimetableRequest timetable) 
        {
            var ret = await _estabilishmentTimetableService.Create(timetable.Adapt<UpsertEstabilishmentTimetableDto>());
            return Ok(ret);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<EstabilishmentTimetableDto>> Update(int id, UpsertEstabilishmentTimetableDto timetable)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
