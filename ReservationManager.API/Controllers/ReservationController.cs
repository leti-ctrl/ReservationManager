using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Request;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetUserReservations(int userId)
        {
            var rezList = await _reservationService.GetUserReservation(userId);
            return Ok(rezList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationDto>> GetById(int id, int userId)
        {
            var rez = await _reservationService.GetById(id, userId);
            if(rez == null)
                return NotFound();

            return Ok(rez);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationDto>> CreateReservation(int userId, ReservationUpsertRequest reservation)
        {
            var created =  await _reservationService.CreateReservation(userId, reservation.Adapt<UpsertReservationDto>());
            return Ok(created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationDto>> UpdateReservation(int id, int userId, UpsertReservationDto reservation)
        {
            var updated =  await _reservationService.UpdateReservation(id, userId, reservation.Adapt<UpsertReservationDto>());
            if (updated == null)
                return NotFound();
            
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteReservation(int id, int userId)
        {
            await _reservationService.DeleteReservation(id, userId);
            return Accepted();
        }
    }
}
