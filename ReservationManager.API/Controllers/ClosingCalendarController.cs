using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Request;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClosingCalendarController : ControllerBase
    {
        private readonly IClosingCalendarService _closingCalendarService;

        public ClosingCalendarController(IClosingCalendarService closingCalendarService)
        {
            _closingCalendarService = closingCalendarService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClosingCalendarDto>>> GetAll()
        {
            var closingCalendarDtos = await _closingCalendarService.GetAll();

            if (!closingCalendarDtos.Any())
                return NoContent();
            
            return Ok(closingCalendarDtos);
        }

        [HttpGet("{typeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClosingCalendarDto>>> GetByType(int typeId)
        {
            var list = await _closingCalendarService.GetByTypeId(typeId);

            if (list.Any())
                return Ok(list);
            return NoContent();
        }

        [HttpGet("dateRange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClosingCalendarDto>>> GetByDateRange(DateOnly start, DateOnly end)
        {
            var list = await _closingCalendarService.GetByDateRange(start, end);
             if(list.Any())
                 return Ok(list);
             return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClosingCalendarDto>> Create(UpsertClosingCalendarRequest timetable) 
        {
            var closingCalendarDto = await _closingCalendarService.Create(timetable.Adapt<UpsertClosingCalendarDto>());
            
            return Ok(closingCalendarDto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClosingCalendarDto>> Update(int id, UpsertClosingCalendarDto timetable)
        {
            var closingCalendarDto = await _closingCalendarService.Update(id, timetable.Adapt<UpsertClosingCalendarDto>());
            return Ok(closingCalendarDto);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            await _closingCalendarService.Delete(id);
            return Accepted();
        }
    }
}
