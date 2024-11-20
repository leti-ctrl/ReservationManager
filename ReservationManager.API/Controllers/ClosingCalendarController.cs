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
        public async Task<ActionResult<IEnumerable<ClosingCalendarDto>>> GetAllFromToday()
        {
            var model = await _closingCalendarService.GetAllFromToday();
            if(model.Any())
                return Ok(model);
            return NoContent();
        }

        [HttpPost("getFiltered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClosingCalendarDto>>> GetFiltered(ClosingCalendarFilterDto closingCalendarFilterDto)
        {
            var model = await _closingCalendarService.GetFiltered(closingCalendarFilterDto);
            if(model.Any())
                return Ok(model);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClosingCalendarDto>> Create(ClosingCalendarUpsertRequest request) 
        {
            var closingCalendar = await _closingCalendarService.Create(request.Adapt<ClosingCalendarDto>());
            return Ok(closingCalendar);
        }
        
        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClosingCalendarDto>>> BulkCreate(ClosingCalendarBucketRequest request) 
        {
            var closingCalendar = await _closingCalendarService.BulkCreate(request.Adapt<ClosingCalendarBucketDto>());
            return Ok(closingCalendar);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClosingCalendarDto>> Update(int id, ClosingCalendarUpsertRequest request)
        {
            var closingCalendar = await _closingCalendarService.Update(id,request.Adapt<ClosingCalendarDto>());
            return Ok(closingCalendar);
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
