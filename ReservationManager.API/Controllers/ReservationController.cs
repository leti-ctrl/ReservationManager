using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Authorization;
using ReservationManager.API.Controllers.Base;
using ReservationManager.API.Request;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.Employee })]
    public class ReservationController : SessionController
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetUserReservations()
        {
            var rezList = await _reservationService.GetUserReservation(GetSession());
            return Ok(rezList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationDto>> GetById(int id)
        {
            var rez = await _reservationService.GetById(id, GetSession());
            if(rez == null)
                return NotFound();

            return Ok(rez);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationDto>> CreateReservation(ReservationUpsertRequest reservation)
        {
            var created =  await _reservationService.CreateReservation(GetSession(), reservation.Adapt<UpsertReservationDto>());
            return Ok(created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationDto>> UpdateReservation(int id, UpsertReservationDto reservation)
        {
            var updated =  await _reservationService.UpdateReservation(id, GetSession(), reservation.Adapt<UpsertReservationDto>());
            if (updated == null)
                return NotFound();
            
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            await _reservationService.DeleteReservation(id, GetSession());
            return Accepted();
        }
    }
}
