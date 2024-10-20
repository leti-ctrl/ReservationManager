using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Request;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationTypeController : ControllerBase
    {
        private readonly IReservationTypeService _reservationTypeService;

        public ReservationTypeController(IReservationTypeService reservationTypeService)
        {
            this._reservationTypeService = reservationTypeService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ReservationTypeDto>>> GetAllReservationTypes()
        {
            var reservationTypes = await _reservationTypeService.GetAllReservationType();

            if (reservationTypes == null)
                return NoContent();
            return Ok(reservationTypes);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationTypeDto>> CreateReservationType(ReservationTypeUpsertRequest request)
        {
            var created = await _reservationTypeService.CreateReservationType(request.Code, request.StartTime, request.EndTime);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationTypeDto>> UpdateReservationType(int id, ReservationTypeUpsertRequest reservation)
        {
            var updated = await _reservationTypeService.UpdateReservationType(id, reservation.Code, reservation.StartTime, reservation.EndTime);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteReservationType(int id)
        {
            await _reservationTypeService.DeleteReservationType(id);
            return Accepted();
        }
    }
}
