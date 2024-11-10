using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Request;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingTimetableController : ControllerBase
    {
        private readonly IBuildingTimetableService _buildingTimetableService;

        public BuildingTimetableController(IBuildingTimetableService buildingTimetableService)
        {
            _buildingTimetableService = buildingTimetableService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BuildingTimetableDto>>> GetAll()
        {
            var timetableDtos = await _buildingTimetableService.GetAll();

            if (!timetableDtos.Any())
                return NoContent();
            
            return Ok(timetableDtos);
        }

        [HttpGet("{typeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<IEnumerable<BuildingTimetableDto>>> GetByType(int typeId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("dateRange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult<IEnumerable<BuildingTimetableDto>>> GetByDateRange(DateOnly start, DateOnly end)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BuildingTimetableDto>> Create(UpsertBuildingTimetableRequest timetable) 
        {
            var buildingTimetableDto = await _buildingTimetableService.Create(timetable.Adapt<UpsertEstabilishmentTimetableDto>());
            
            return Ok(buildingTimetableDto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BuildingTimetableDto>> Update(int id, UpsertEstabilishmentTimetableDto timetable)
        {
            var buildingTimetableDto = await _buildingTimetableService.Update(id, timetable.Adapt<UpsertEstabilishmentTimetableDto>());
            return Ok(buildingTimetableDto);
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
